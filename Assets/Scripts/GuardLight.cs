using System;
using System.Collections;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardLight : MonoBehaviour
{   
    public Action onFrauCaught; // Event to notify when the player is caught

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            
            if (CharacterController2D.Instance.currentPickup != null &&
                CharacterController2D.Instance.currentPickup is Painting) {
                CharacterController2D.Instance.setCanMove(false);
                CharacterController2D.Instance.animationController.PlayGetCaughtAnimation();
                
            }
        }
    }

    public void StopMovement(float duration) {
        IEnumerator coroutine = DisableLight(duration);
        StartCoroutine(coroutine);
    }
    

    private IEnumerator DisableLight(float duration) {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        polygonCollider.enabled = true;
    }
}