using UnityEngine;
using UnityEngine.Events;
public class CharacterController2D : MonoBehaviour
{
	private Rigidbody2D body;

	private float horizontal;
	private float vertical;
	private float moveLimiter = 0.7f;

	[SerializeField] private float runSpeed = 20.0f;

	void Start ()
	{
		body = GetComponentInChildren<Rigidbody2D>();
	}

	void Update()
	{
		// Gives a value between -1 and 1
		horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
		vertical = Input.GetAxisRaw("Vertical"); // -1 is down
	}

	void FixedUpdate()
	{
		if (horizontal != 0 && vertical != 0) // Check for diagonal movement
		{
			// limit movement speed diagonally, so you move at 70% speed
			horizontal *= moveLimiter;
			vertical *= moveLimiter;
		} 

		body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
	}
}