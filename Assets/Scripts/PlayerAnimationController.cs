using UnityEngine;
using System.Collections;

namespace Intertables
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;
        private string currentAnimation = ""; // Tracks the current animation
        private string lastDirection = "";   // Tracks the last direction for idle or stealing animations
        
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
            // Lock player movement

            

            // Ensure the animation state exists
            if (!animator.HasState(0, Animator.StringToHash("Player_get_caught")))
            {
                Debug.LogError("Animation state 'Player_get_caught' not found!");
                
            }

            // Play the get caught animation
            animator.Play("Player_get_caught");

            // Wait for the animation to complete (adjust the duration to match your animation clip)
            

            // Unlock player movement
           
        }

        public IEnumerator PlayLevelCompletionAnimation(System.Action lockMovementCallback, System.Action unlockMovementCallback)
        {
            // Lock player movement
            lockMovementCallback?.Invoke();

            Debug.Log("Playing level completion animation: no_no_last");

            // Ensure the animation state exists
            if (!animator.HasState(0, Animator.StringToHash("no_no_last")))
            {
                Debug.LogError("Animation state 'no_no_last' not found!");
                yield break;
            }

            // Play the no_no_last animation
            animator.Play("no_no_last");

            // Wait for the animation to complete (adjust duration to match your clip)
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Unlock player movement
            unlockMovementCallback?.Invoke();
        }
        // Starts the stealing animation and locks movement for the duration
        public IEnumerator PlayStealingAnimation(System.Action lockMovementCallback, System.Action unlockMovementCallback)
        {
            // Lock movement
            lockMovementCallback?.Invoke();

            // Determine which stealing animation to play
            string stealingAnimation = GetStealingAnimation(lastMovementDirection);
            Debug.Log("Playing stealing animation: " + stealingAnimation);
            animator.Play(stealingAnimation);

            yield return new WaitForSeconds(0.52f); // 52 milliseconds = 0.52 seconds

            // Unlock movement
            unlockMovementCallback?.Invoke();
        }

        // Determines stealing animation based on the last direction
        private string GetStealingAnimation(string  lastMovementDirection)
        {
            Debug.Log("Determining stealing animation for lastMovementDirection: " + lastMovementDirection);
            return lastMovementDirection switch
            {
                "Left" => "Stealing_left",
                "Right" => "Stealing_right",
                "Back" => "Stealing_back",
                _ => "Stealing_back" // Default if undefined
            };
        }


        // Plays a specific animation for a given duration
        public IEnumerator PlayAnimationForDuration(string animationName, float duration)
        {
            animator.Play(animationName); // Play the specified animation
            yield return new WaitForSeconds(duration); // Wait for the animation duration
        }

        // Handles mopping animation
        public void PlayMoppingAnimation(MonoBehaviour caller, float duration)
        {
            caller.StartCoroutine(PlayAnimationForDuration("Player_mopping", duration));
        }
    }
}
