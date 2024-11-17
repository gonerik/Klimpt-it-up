using UnityEngine;
using System.Collections;

namespace Intertables
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;
        private string currentAnimation = ""; // Tracks the current animation

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found! Ensure there's an Animator component in the child object.");
            }
        }

        public void PlayWalkAnimation(float horizontal, float vertical, ref string lastDirection, bool canMove)
        {
            // If movement is locked, play idle animation
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
            }
            else if (horizontal > 0)
            {
                targetAnimation = "Player_walk_right";
                lastDirection = "Right";
            }
            else if (vertical > 0)
            {
                targetAnimation = "Player_walk_back";
                lastDirection = "Back";
            }
            else if (vertical < 0)
            {
                targetAnimation = "Walk_front_animation";
                lastDirection = "Front";
            }

            // Only play animation if it's not already playing
            if (currentAnimation != targetAnimation)
            {
                animator.Play(targetAnimation);
                currentAnimation = targetAnimation;
            }
        }

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

        public IEnumerator PlayAnimationForDuration(string animationName, float duration)
        {
            animator.Play(animationName); // Play the specified animation
            yield return new WaitForSeconds(duration); // Wait for the animation duration
        }

        public void PlayMoppingAnimation(MonoBehaviour caller, float duration)
        {
            caller.StartCoroutine(PlayAnimationForDuration("Player_mopping", duration));
        }
    }
}
