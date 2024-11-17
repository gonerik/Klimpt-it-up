using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace Intertables
{
    public class CharacterController2D : MonoBehaviour
    {
        [Header("Movement")] public static CharacterController2D Instance;
        private Rigidbody2D body;
        private bool canMove = true;

        private float horizontal;
        private float vertical;
        private float moveLimiter = 0.7f;

        private float runSpeed;
        [SerializeField] private float maxRunSpeed = 8f;
        [SerializeField] private float minRunSpeed = 4f;

        [Header("Interactable Variables")] [SerializeField]
        private float interactionRange = 2f;

        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        public Interactable currentInteractable;

        [Header("PickUpObjects")] public PickUpObjects currentPickup;
        public Vector3 offset = new Vector3(0, -1, 0);
        public float pickUpMoveSpeed = 5f;
        public float amplitude = 0.2f;
        private float floatTimer = 0f;
        public float floatSpeed = 2f;

        [Header("MopUsage")] [SerializeField] private GameObject puddle;
        private GameObject CurrentPuddle;
        [SerializeField] private Tilemap tilemap;

        [Header("Stealing")] private bool isHoldingPickUpObject = false;
        public bool GetIsHoldingPickUpObject() => isHoldingPickUpObject;
        public void SetIsHoldingPickUpObject(bool isHolding) => isHoldingPickUpObject = isHolding;

        [Header("Animation")] private string lastDirection = "Front";
        private PlayerAnimationController animationController;

        public void settoMaxSpeed() => runSpeed = maxRunSpeed;
        public void settoMinSpeed() => runSpeed = minRunSpeed;
        public void setCanMove(bool value) => canMove = value;

        private void Start()
        {
            body = GetComponent<Rigidbody2D>();
            animationController = GetComponent<PlayerAnimationController>();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("CharacterController2D already exists!");
            }

            runSpeed = maxRunSpeed;
        }

        private void Update()
        {
            // Movement input
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            DetectInteractable();
            HandleInteraction();

            if (isHoldingPickUpObject && currentPickup != null)
            {
                HandleCarriedObject();
            }

            if (Input.GetKeyDown("1"))
            {
                SpawnPuddle();
            }

            // Delegate walking animation handling to PlayerAnimationController
            animationController.PlayWalkAnimation(horizontal, vertical, ref lastDirection, canMove);
        }

        private void FixedUpdate()
            {
                if (canMove)
                {
                    Vector2 movement = new Vector2(horizontal, vertical);

                    if (movement.magnitude > 1)
                    {
                        movement *= moveLimiter;
                    }

                    body.velocity = movement * runSpeed;
                }
                else
                {
                    body.velocity = Vector2.zero; // Stop all movement when canMove is false
                }
            }


            private void DetectInteractable()
            {
                Collider2D[] hitColliders =
                    Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

                if (hitColliders.Length > 0)
                {
                    float closestDistance = float.MaxValue;
                    Interactable closestInteractable = null;

                    foreach (Collider2D collider in hitColliders)
                    {
                        Interactable interactable = collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            float distance = Vector2.Distance(transform.position, collider.transform.position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestInteractable = interactable;
                            }
                        }
                    }

                    currentInteractable = closestInteractable;
                }
                else
                {
                    currentInteractable = null;
                }
            }

            private void HandleInteraction()
            {
                if (Input.GetKeyDown(interactionKey))
                {
                    if (isHoldingPickUpObject && currentPickup is MopSign)
                    {
                        Debug.Log("Put down a mop sign.");
                        currentInteractable = currentPickup;
                        SpawnMopSign();
                        if (!isHoldingPickUpObject) settoMaxSpeed();
                    }
                    else if (currentInteractable != null)
                    {
                        if (isHoldingPickUpObject && currentInteractable is Storage)
                        {
                            currentInteractable.Interact();
                            Debug.Log("Interacted with storage.");
                        }
                        else if (currentInteractable is HidingSpot)
                        {
                            currentInteractable.Interact();
                        }
                        else if (!isHoldingPickUpObject && currentInteractable is Painting)
                        {
                            currentPickup = currentInteractable.GetComponent<PickUpObjects>();
                            currentInteractable.Interact();
                            isHoldingPickUpObject = true;
                            Debug.Log("Picked up a painting.");
                        }
                        else if (!isHoldingPickUpObject && currentInteractable is MopSign)
                        {
                            currentPickup = currentInteractable.GetComponent<MopSign>();
                            currentInteractable.Interact();
                            isHoldingPickUpObject = true;
                        }
                    }
                }
            }

            private void HandleCarriedObject()
            {
                if (currentPickup == null) return;

                Vector3 targetPosition = transform.position + offset;
                floatTimer += Time.deltaTime * floatSpeed;
                targetPosition.y += Mathf.Sin(floatTimer) * amplitude;

                float distance = Vector3.Distance(currentPickup.transform.position, targetPosition);
                if (distance > 0.1f)
                {
                    currentPickup.transform.position = Vector3.Lerp(currentPickup.transform.position, targetPosition,
                        pickUpMoveSpeed * Time.deltaTime);
                }
                else
                {
                    currentPickup.transform.position = targetPosition;
                }
            }

            private void SpawnPuddle()
            {
                if (CurrentPuddle != null)
                {
                    Destroy(CurrentPuddle);
                }

                Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
                Vector3 tileCenterPosition = tilemap.GetCellCenterWorld(cellPosition);

                CurrentPuddle = Instantiate(puddle, tileCenterPosition, Quaternion.identity);

                // Start coroutine to play the animation and lock movement
                StartCoroutine(PlayPuddleAnimationAndLockMovement());
            }

            private IEnumerator PlayPuddleAnimationAndLockMovement()
            {
                // Lock movement
                setCanMove(false);

                // Play mopping animation
                animationController.PlayMoppingAnimation(this, 1.1f); // Use 1.1 seconds for animation duration

                // Wait for animation to finish
                yield return new WaitForSeconds(1.1f);

                // Unlock movement
                setCanMove(true);
            }

            private void SpawnMopSign()
            {
                if (currentPickup is not MopSign) return;

                Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
                Vector3 tileCenterPosition = tilemap.GetCellCenterWorld(cellPosition);

                currentPickup.GetComponent<Collider2D>().enabled = true;
                currentPickup.transform.position = tileCenterPosition;
                currentPickup.gameObject.layer = LayerMask.NameToLayer("Interactable");

                currentPickup = null;
                isHoldingPickUpObject = false;
            }
            
        }
}
