using Intertables;
using UnityEngine;

public class GuardAnimatorController : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower; // Reference to the path-following logic
    [SerializeField] private GameObject guardLightPivot; // Pivot point for rotating the guard light
    [SerializeField] private GuardLight guardLight; // Reference to the guard's light
    private Animator animator;
    private string currentSide; // Tracks the guard's current facing direction
    private bool isFrauCaught = false; // Tracks if the player (Frau) has been caught
    private bool isSlipping = false; // Tracks if the guard is slipping

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator) Debug.LogError("Animator component not found!");
    }

    private void OnEnable()
    {
        guardLight.onFrauCaught += PlayCatchFrau; // Subscribe to FrauCaught event
        pathFollower.slip += PlaySlipAnimation; // Subscribe to slip event
    }

    private void OnDisable()
    {
        guardLight.onFrauCaught -= PlayCatchFrau; // Unsubscribe from FrauCaught event
        pathFollower.slip -= PlaySlipAnimation; // Unsubscribe from slip event
    }

    private void Update()
    {
        if (!isFrauCaught && !isSlipping)
        {
            UpdateGuardMovementAndAnimation();
        }
    }

    private void UpdateGuardMovementAndAnimation()
    {
        // Get the current waypoint the guard is heading toward
        if (pathFollower.waypoints.Length > 1)
        {
            GameObject currentGuardWaypoint = pathFollower.waypoints[pathFollower.GetCurrentWaypointIndex()];
            Vector3 waypointPosition = currentGuardWaypoint.transform.position;
            Vector3 guardPosition = transform.position;
            // Determine whether to play horizontal or vertical walking animations
            float horizontalDifference = Mathf.Abs(waypointPosition.x - guardPosition.x);

            if (horizontalDifference > 0.1f) // Horizontal movement
            {
                if (waypointPosition.x > guardPosition.x)
                {
                    PlayAnimation("Guard_walt_right", "Right", -90);
                }
                else
                {
                    PlayAnimation("Guard_walk_left", "Left", 90);
                }
            }
            else // Vertical movement
            {
                if (waypointPosition.y > guardPosition.y)
                {
                    PlayAnimation("Guard_walk_back", "Back", 0);
                }
                else
                {
                    PlayAnimation("Guard_walk_front", "Front", 180);
                }
            }
        }
    }

    private void PlayAnimation(string animationName, string side, float lightRotationZ)
    {
        if (currentSide != side) // Avoid redundant animation plays
        {
            animator.Play(animationName);
            currentSide = side;
            guardLightPivot.transform.rotation = Quaternion.Euler(0, 0, lightRotationZ);
        }
    }

    private void PlayCatchFrau()
    {
        Debug.Log("Guard caught Frau!");

        pathFollower.stopTimer += 4f; // Add delay to stopTimer
        pathFollower.isStopped = true; // Stop guard movement
        isFrauCaught = true;

        // Play the correct "pointing" animation based on the current side
        string pointingAnimation = currentSide switch
        {
            "Right" => "Guard_pointing_right",
            "Left" => "Guard_pointing_left",
            "Back" => "Guard_pointing_back",
            _ => "Guard_pointing_front", // Default to front if side is undefined
        };
        animator.Play(pointingAnimation);
        
    }

    private void PlaySlipAnimation()
    {
        Debug.Log("Guard slipping!");

        isSlipping = true;
        animator.Play("Guard_slippering_animation");
    }

    // Called at the end of the slipping animation via Animation Event
    private void OnSlipAnimationEnd()
    {
        Debug.Log("Guard finished slipping.");
        isSlipping = false;
    }
}
