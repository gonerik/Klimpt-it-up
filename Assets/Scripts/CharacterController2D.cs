using Intertables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Intertables
{
    public class CharacterController2D : MonoBehaviour
    {
        [Header("Movement")] 
        public static CharacterController2D Instance;
        private Rigidbody2D body;
        private bool canMove = true;

        private float horizontal;
        private float vertical;
        private float moveLimiter = 0.7f;

        private float runSpeed;
        [SerializeField] private float maxRunSpeed = 8f;
        [SerializeField] private float minRunSpeed = 4f;

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

	[FormerlySerializedAs("Puddle")]
    [Header("MopUsage")]
	[SerializeField] private GameObject puddle;
	private GameObject CurrentPuddle;

	[SerializeField] private Tilemap tilemap;
        [Header("Stealing")]
        private bool isHoldingPickUpObject = false;
        
        public bool GetIsHoldingPickUpObject() => isHoldingPickUpObject;
        public void SetIsHoldingPickUpObject(bool isHolding) => isHoldingPickUpObject = isHolding;
        [Header("Animation")]
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private string lastDirection = "Front"; // Keeps track of the last direction

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("CharacterController2D already exists!");
            }
            body = GetComponent<Rigidbody2D>();
            runSpeed = maxRunSpeed;
            DialogueManager.instance.StartDialogue(DialogueManager.instance.dialogueLines);
        }

        public void settoMaxSpeed()
        {
            runSpeed = maxRunSpeed;
        }

        public void settoMinSpeed()
        {
            runSpeed = minRunSpeed;
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
            if (isHoldingPickUpObject && currentPickup != null)
            {
                HandleCarriedObject();
            }

			if (Input.GetKeyDown("1")) {
				SpawnPuddle();
			}

            
            
            HandleAnimation(); // Call animation handler

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

            if (canMove)
            {
                body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            }
            else
            {
                body.velocity = Vector2.zero;
            }
        }

        void HandleInteraction()
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
                        // Interact with storage to drop the painting
                        currentInteractable.Interact();

                        Debug.Log("Interacted with storage.");
                    }
                    else if (currentInteractable is HidingSpot)
                    {
                        // Interact with storage to deposit or withdraw the painting
                        currentInteractable.Interact();
                    }
                    else if (!isHoldingPickUpObject && currentInteractable is Painting)
                    {
                        // Pick up the object
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
		
		private void SpawnPuddle() {
			if (CurrentPuddle != null)
			{
				Destroy(CurrentPuddle);
			}

			Vector3Int cellPosition = tilemap.WorldToCell(transform.position); 
			Vector3 tileCenterPosition = tilemap.GetCellCenterWorld(cellPosition);

			CurrentPuddle = Instantiate(puddle, tileCenterPosition, Quaternion.identity);
		}

		private void SpawnMopSign() {
            if (currentPickup is not MopSign) return;
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position); 
            Vector3 tileCenterPosition = tilemap.GetCellCenterWorld(cellPosition);
            currentPickup.GetComponent<Collider2D>().enabled = true;
            currentPickup.transform.position = tileCenterPosition;
            currentPickup.GameObject().layer = LayerMask.NameToLayer("Interactable");
            currentPickup = null;
            isHoldingPickUpObject = false;
        }

        public void setCanMove(bool value)
        {
            canMove = value;
        }
    private void HandleAnimation()
    {
        if (horizontal < 0)
        {
            animator.Play("Player_walk_left"); // Play left walk animation
            lastDirection = "Left";            // Remember last direction
            
        }
        else if (horizontal > 0)
        {
            animator.Play("Player_walk_right"); // Play right walk animation
            lastDirection = "Right";            // Remember last direction
        }
        else if (vertical > 0)
        {
            animator.Play("Player_walk_back"); // Play back walk animation
            lastDirection = "Back";           // Remember last direction
        }
        else if (vertical < 0)
        {
            animator.Play("Walk_front_animation"); // Play front walk animation
            lastDirection = "Front";               // Remember last direction
        }
        else
        {
            // If no movement, play idle animation
            PlayIdleAnimation();
        }
    }

    private void PlayIdleAnimation()
    {
        if (lastDirection == "Left")
        {
            animator.Play("Player_idle_left"); // Play left idle animation
        }
        else if (lastDirection == "Back")
        {
            animator.Play("Player_idle_back"); // Play back idle animation
        }
        else if (lastDirection == "Front")
        {
            animator.Play("Walk_front_animation"); // Play front idle animation
        }
        else if (lastDirection == "Right")
        {
            animator.Play("Player_idle_right"); // Play front idle animation
        }
        else
        {
            // If no movement, play idle animation
            animator.Play("Player_idle");
        }
    }


}
}
