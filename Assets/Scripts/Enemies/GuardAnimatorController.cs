using Intertables;
using UnityEngine;

public class GuardAnimatorController : MonoBehaviour
{
    private Animator animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastVertical  = "LastVertical";
    private const string _lastHorizontal = "LastHorizontal";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator) Debug.LogError("Animator component not found!");
    }
    public void setAxis(float horizontal, float vertical)
    {
        animator.SetFloat(_horizontal, horizontal);
        animator.SetFloat(_vertical, vertical);
    }
    public void setLastAxis(float lastHorizontal, float lastVertical)
    {
        animator.SetFloat(_lastVertical, lastHorizontal);
        animator.SetFloat(_lastHorizontal, lastVertical);
    }

    private void PlayCatchFrau()
    {
        animator.SetTrigger("Catch");
    }

    private void PlaySlipAnimation()
    {
        animator.SetTrigger("Slip");
    }
    
}
