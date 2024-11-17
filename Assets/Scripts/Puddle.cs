using UnityEngine;

public class Puddle : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            PathFollower guardPath = other.GetComponent<PathFollower>();
            GuardLight guardLight = other.GetComponentInChildren<GuardLight>();
            guardPath.StopMovement(3f);
            guardLight.StopMovement(3f);
            guardPath.slip.Invoke();
            Destroy(gameObject);
        }
    }
}
