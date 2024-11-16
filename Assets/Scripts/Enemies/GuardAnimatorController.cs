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
        animator.Play("Guard_walk_front");
    }
}
