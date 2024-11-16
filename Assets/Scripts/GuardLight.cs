using System.Collections;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardLight : MonoBehaviour
{   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            print("hello");
            if (CharacterController2D.Instance.GetIsHoldingPainting()) {
                IEnumerator coroutine = ReloadScene(0.5f);
                StartCoroutine(coroutine);
            }
        }
        if (other.tag == "MopSign") {
            PathFollower pathFollower = GetComponentInParent<PathFollower>();
            pathFollower.ReversePath();
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
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
