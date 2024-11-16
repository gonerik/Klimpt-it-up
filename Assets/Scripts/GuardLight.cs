using UnityEngine;

public class GuardLight : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            print("Player detected");
        }
    }
}
