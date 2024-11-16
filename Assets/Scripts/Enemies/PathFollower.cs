using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public GameObject[] waypoints; // Waypoints set from the Editor
    public float speed = 2f; // Movement speed
    public float reachDistance = 0.1f; // Distance to consider a waypoint "reached"
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private bool isReversing = false; // Flag to indicate if the path is being traversed in reverse
    private bool isStopped = false;
    private float stopTimer = 0f;
    private float puddleImmunityTimer = 0f;

    private void Update()
    {
        if (puddleImmunityTimer > 0f) {
            puddleImmunityTimer -= Time.deltaTime;
        }

        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                isStopped = false; // Resume movement
            }
            return; // Skip movement logic while stopped
        }

        Move();
    }

    public void StopMovement(float duration) {
        isStopped = true;
        stopTimer = duration;
        if (puddleImmunityTimer > 0f) {
            isStopped = false;
        } else puddleImmunityTimer = 10f;
    }

    public void Move() {
        if (waypoints.Length == 0) return;

        // Move towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex].transform.position;
        Vector3 movementDirection = targetPosition - transform.position;
        transform.position += movementDirection.normalized * speed * Time.deltaTime;

        // Rotate to face the waypoint
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Check if the waypoint is reached
        if (Vector3.Distance(transform.position, targetPosition) < reachDistance)
        {
            // Determine next waypoint index
            if (!isReversing)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    isReversing = true;
                    currentWaypointIndex = waypoints.Length - 2; // Start going back from the second-to-last waypoint
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    isReversing = false;
                    currentWaypointIndex = 1; // Start going forward from the second waypoint
                }
            }
        }
    }
}
