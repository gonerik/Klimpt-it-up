using Intertables;
using UnityEngine;
using UnityEngine.Events;
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
	private Interactable currentInteractable;
	
	[Header("PickUpObjects")]
	private Interactable currentPickup;
	public Vector3 offset = new Vector3(0, -1, 0);
	public float pickUpMoveSpeed = 5f;
	public float amplitude = 0.2f;
	private float floatTimer = 0f;
	public float floatSpeed = 2f;

	[Header("Stealing")]
	private bool isHoldingAPainting = false;

	void Start ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogError("CharacterController2D already exists!");
		}
		body = GetComponentInChildren<Rigidbody2D>();
	}

	void Update()
	{
		// Gives a value between -1 and 1
		horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
		vertical = Input.GetAxisRaw("Vertical"); // -1 is down
		DetectInteractable();

		if (currentInteractable != null && Input.GetKeyDown(interactionKey))
		{
			if (currentInteractable.pickable)
			{
				currentPickup = currentInteractable;
			}
			currentInteractable.Interact();
		}
		
		{
			if (currentPickup != null)
			{
				// Smoothly move the object towards the player's back
				Vector3 targetPosition = transform.position + offset;
				floatTimer += Time.deltaTime * floatSpeed;
				targetPosition.y = Mathf.Sin(floatTimer) * amplitude +targetPosition.y;
				float distance = Vector3.Distance(transform.position, targetPosition);
				

				if (distance > 0.1f)
				{
					currentPickup.transform.position = Vector3.Lerp(currentPickup.transform.position, targetPosition, pickUpMoveSpeed * Time.deltaTime);
				}
				else
				{
					currentPickup.transform.position = targetPosition; // Snap to position once close enough
				}
			}
			
		}
	}
	void DetectInteractable()
	{
		if (currentPickup != null) return;
		// Cast a small sphere to detect objects within interaction range
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

		if (hits.Length > 0)
		{
			// Assume the first hit is the closest interactable object
			currentInteractable = hits[0].GetComponent<Interactable>();
		}
		else
		{
			currentInteractable = null;
		}
	}

	void OnDrawGizmosSelected()
	{
		// Visualize the interaction range in the editor
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, interactionRange);
	}

	void FixedUpdate()
	{
		if (horizontal != 0 && vertical != 0) // Check for diagonal movement
		{
			// limit movement speed diagonally, so you move at 70% speed
			horizontal *= moveLimiter;
			vertical *= moveLimiter;
		}

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
}