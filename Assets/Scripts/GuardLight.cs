using System.Collections;
using UnityEngine;

public class GuardLight : MonoBehaviour
{   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            print("Player detected");
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
}
