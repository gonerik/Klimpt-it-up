using UnityEngine;

public class GuardAnimatorController : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GameObject guardLightPivot;
    [SerializeField] private GuardLight guardLight;
    private Animator animator;
    private string currentSide;
    private bool frauCaught = false;
    private bool slipping = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        guardLight.FrauCaught += PlayCatchFrau;
        pathFollower.slip += Slip;
    }

    void OnDisable()
    {
        guardLight.FrauCaught -= PlayCatchFrau;
        pathFollower.slip -= Slip;
    }

    void Update()
    {
        if (!frauCaught && !slipping) {
            GameObject currentGuardWaypoint = pathFollower.waypoints[pathFollower.GetCurrentWaypointIndex()];
            float XAbs = Mathf.Abs(currentGuardWaypoint.transform.position.x - transform.position.x);
            if (XAbs > 0.1) {
                if (currentGuardWaypoint.transform.position.x > transform.position.x) {
                    animator.Play("Guard_walt_right");
                    currentSide = "Right";
                    guardLightPivot.transform.rotation = Quaternion.Euler(0, 0, -90);
                } else {
                    animator.Play("Guard_walk_left");
                    currentSide = "Left";
                    guardLightPivot.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            } else {
                if (currentGuardWaypoint.transform.position.y > transform.position.y) {
                    animator.Play("Guard_walk_back");
                    currentSide = "Back";
                    guardLightPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                } else {
                    animator.Play("Guard_walk_front");
                    currentSide = "Front";
                    guardLightPivot.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }
        }
    }

    private void PlayCatchFrau() {
        frauCaught = true;
        if (currentSide == "Right") {
            animator.Play("Guard_pointing_right");
        } else if (currentSide == "Left") {
            animator.Play("Guard_pointing_left");
        } else if (currentSide == "Back") {
            animator.Play("Guard_pointing_back");
        } else {
            animator.Play("Guard_pointing_front");
        }
    }

    private void Slip() {
        slipping = true;
        animator.Play("Guard_slippering_animation");
    }

    private void OnSlipAnimationEnd() {
        slipping = false;
    }
}
