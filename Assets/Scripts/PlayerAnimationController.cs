using UnityEngine;
using System.Collections;

namespace Intertables
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;
        
        private string lastMovementDirection = "Front"; // Tracks the last movement direction for stealing animations
        private const string _horizontal = "Horizontal";
        private const string _vertical = "Vertical";
        private const string _lastVertical  = "LastVertical";
        private const string _lastHorizontal = "LastHorizontal";
        
        
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found! Ensure there's an Animator component in the child object.");
            }
        }

        public void setLastAxis(float horizontal, float vertical)
        {
            Debug.Log("setting last axis");
            animator.SetFloat(_lastHorizontal, horizontal);
            animator.SetFloat(_lastVertical, vertical);
        }

        public void setAxis(float horizontal, float vertical)
        {
            Debug.Log("setting axis");
            animator.SetFloat(_horizontal, horizontal);
            animator.SetFloat(_vertical, vertical);
        }
        
        public void PlayGetCaughtAnimation()
        {
            animator.SetTrigger("GetCaught");
        }

        public void PlayLevelCompletionAnimation()
        {
            animator.SetTrigger("Win");
        }
        public void PlayStealingAnimation()
        {
            animator.SetTrigger("Steal");

        }
        public void PlayMoppingAnimation()
        {
            animator.SetTrigger("Mop");
        }

        public void PlayHidingAnimation()
        {
            animator.SetTrigger("Hide");
        }
    }
}
