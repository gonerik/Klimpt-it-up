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
                IEnumerator coroutine = ReloadScene(3f);
                StartCoroutine(coroutine);
                onFrauCaught?.Invoke();
            }
        }
    }

    public void StopMovement(float duration) {
        IEnumerator coroutine = DisableLight(duration);
        StartCoroutine(coroutine);
    }

    public IEnumerator ReloadScene(float duration) // Changed to public
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DisableLight(float duration) {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        polygonCollider.enabled = true;
    }
}