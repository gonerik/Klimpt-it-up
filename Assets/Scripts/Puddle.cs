using UnityEngine;

public class Puddle : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            Guard guardPath = other.GetComponentInParent<Guard>();
            GuardLight guardLight = guardPath.GetComponentInChildren<GuardLight>();
            if (guardPath.puddleImmunityTimer <= 0f) {
                guardPath.StopMovement(3f);
                
                
                Destroy(gameObject);
            }
        }
    }
}
