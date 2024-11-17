using System.Collections;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardLight : MonoBehaviour
{   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            
            if (CharacterController2D.Instance.currentPickup != null &&
                CharacterController2D.Instance.currentPickup is Painting) {
                IEnumerator coroutine = ReloadScene(3f);
                StartCoroutine(coroutine);
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

    private IEnumerator ReloadScene(float duration) {
        CharacterController2D.Instance.setCanMove(false);
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
