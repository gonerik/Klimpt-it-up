using UnityEngine;

public class Puddle : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        print("hello");
        if (other.tag == "Enemy") {
            PathFollower guardPath = other.GetComponent<PathFollower>();
            guardPath.StopMovement(3f);
        }
    }
}
