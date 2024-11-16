using UnityEngine;

public class GuardAnimatorController : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GuardLight guardLight;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameObject currentGuardWaypoint = pathFollower.waypoints[pathFollower.GetCurrentWaypointIndex()];
        float XAbs = Mathf.Abs(currentGuardWaypoint.transform.position.x - transform.position.x);
        if (XAbs > 0.1) {
            if (currentGuardWaypoint.transform.position.x > transform.position.x) {
                animator.Play("Guard_walt_right");
            } else {
                animator.Play("Guard_walk_left");
            }
        } else {
            if (currentGuardWaypoint.transform.position.y > transform.position.y) {
                animator.Play("Guard_walk_back");
            } else {
                animator.Play("Guard_walk_front");
            }
        }
    }
}
