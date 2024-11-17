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

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found! Ensure there's an Animator component in the child object.");
            }
        }

        // Handles walking animations
        public void PlayWalkAnimation(float horizontal, float vertical, ref string lastDirection, bool canMove)
        {
            if (!canMove || (horizontal == 0 && vertical == 0))
            {
                // Play idle animation based on last direction
                string idleAnimation = GetIdleAnimation(lastDirection);
                if (currentAnimation != idleAnimation)
                {
                    animator.Play(idleAnimation);
                    currentAnimation = idleAnimation; // Update current animation state
                }
                return;
            }

            // Determine which walking animation to play
            string targetAnimation = "";

            if (horizontal < 0)
            {
                targetAnimation = "Player_walk_left";
                lastDirection = "Left";
                lastMovementDirection = "Left"; 
            }
            else if (horizontal > 0)
            {
                targetAnimation = "Player_walk_right";
                lastDirection = "Right";
                lastMovementDirection = "Right"; 
            }
            else if (vertical > 0)
            {
                targetAnimation = "Player_walk_back";
                lastDirection = "Back";
                lastMovementDirection = "Back"; 
            }
            else if (vertical < 0)
            {
                targetAnimation = "Walk_front_animation";
                lastDirection = "Front";
                lastMovementDirection = "Front"; 
            }

            // Only play the animation if it's not already playing
            if (currentAnimation != targetAnimation)
            {
                animator.Play(targetAnimation);
                currentAnimation = targetAnimation;
            }
        }

        // Determines idle animation based on the last direction
        private string GetIdleAnimation(string lastDirection)
        {
            return lastDirection switch
            {
                "Left" => "Player_idle_left",
                "Right" => "Player_idle_right",
                "Back" => "Player_idle_back",
                "Front" => "Walk_front_animation", // Front idle animation
                _ => "Player_idle", // Default idle animation
            };
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
