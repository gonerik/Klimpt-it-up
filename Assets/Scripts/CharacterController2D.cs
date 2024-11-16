using UnityEngine;

namespace Intertables
{
    public class CharacterController2D : MonoBehaviour
    {
        [Header("Movement")]
        public static CharacterController2D Instance;
        private Rigidbody2D body;

        private float horizontal;
        private float vertical;
        private float moveLimiter = 0.7f;

        [SerializeField] private float runSpeed = 20.0f;

        [Header("Interactable Variables")]
        [SerializeField] private float interactionRange = 2f; // How close you need to be to interact
        [SerializeField] private LayerMask interactableLayer; // Set this to the layer of interactable objects
        [SerializeField] private KeyCode interactionKey = KeyCode.E; // Key to press for interaction
        public Interactable currentInteractable;

        [Header("PickUpObjects")]
        public PickUpObjects currentPickup;
        public Vector3 offset = new Vector3(0, -1, 0);
        public float pickUpMoveSpeed = 5f;
        public float amplitude = 0.2f;
        private float floatTimer = 0f;
        public float floatSpeed = 2f;

        [Header("Stealing")]
        private bool isHoldingPainting = false;
        

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
                Instance = this;
                Debug.LogError("CharacterController2D already exists!");
            }
            body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SettingsMenu.instance.Pause();
            }
            // Movement input
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            DetectInteractable();
            // Interaction
            HandleInteraction();

            // Carrying the painting
            if (isHoldingPainting && currentPickup != null)
            {
                HandleCarriedObject();
            }
        }
        void DetectInteractable()
        {
            // Find objects within the interaction range
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

            if (hitColliders.Length > 0)
            {
                // Get the closest interactable
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
                currentInteractable = null; // No interactables in range
            }
        }

        void FixedUpdate()
        {
            // Movement logic
            if (horizontal != 0 && vertical != 0)
            {
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            // Adjust offset for carried object based on player direction
            if (horizontal < 0)
            {
                offset.x = 0.6f;
            }
            else if (horizontal > 0)
            {
                offset.x = -0.6f;
            }

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }

        void HandleInteraction()
        {
            if (currentInteractable != null && Input.GetKeyDown(interactionKey))
            {
                if (!isHoldingPainting && currentInteractable.pickable)
                {
                    // Pick up the object
                    currentPickup = currentInteractable.GetComponent<PickUpObjects>();
                    currentInteractable.Interact();
                    isHoldingPainting = true;

                    Debug.Log("Picked up a painting.");
                }
                else if (isHoldingPainting && currentInteractable is Storage)
                {
                    // Interact with storage to drop the painting
                    currentInteractable.Interact();

                    // Make the painting disappear
                    currentPickup.gameObject.SetActive(false);
                    currentPickup = null;
                    isHoldingPainting = false;

                    Debug.Log("Stored the painting.");
                }
            }
        }

        void HandleCarriedObject()
        {
            if (currentPickup == null) return;

            // Smoothly move the object towards the player's back
            Vector3 targetPosition = transform.position + offset;
            floatTimer += Time.deltaTime * floatSpeed;
            targetPosition.y += Mathf.Sin(floatTimer) * amplitude; // Add floating effect

            float distance = Vector3.Distance(currentPickup.transform.position, targetPosition);
            if (distance > 0.1f)
            {
                currentPickup.transform.position = Vector3.Lerp(currentPickup.transform.position, targetPosition, pickUpMoveSpeed * Time.deltaTime);
            }
            else
            {
                currentPickup.transform.position = targetPosition;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
