using System.Collections;
using UnityEngine;

public class Puddle : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            PathFollower guardPath = other.GetComponent<PathFollower>();
            GuardLight guardLight = other.GetComponentInChildren<GuardLight>();
            guardPath.StopMovement(3f);
            StartCoroutine(guardLight.DisableLight(3f));
            Destroy(gameObject);
        }
    }
}
