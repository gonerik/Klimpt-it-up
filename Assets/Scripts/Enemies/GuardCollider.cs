using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCollider : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MopSign") {
            pathFollower.ReversePath();
        }
    }
}
