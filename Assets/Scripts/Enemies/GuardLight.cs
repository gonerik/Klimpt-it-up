using System;
using System.Collections;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardLight : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    [SerializeField] private Guard guard;
    private void Start()
    {
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            
            if (CharacterController2D.Instance.currentPickup != null &&
                CharacterController2D.Instance.currentPickup is Painting) {
                CharacterController2D.Instance.setCanMove(false);
                CharacterController2D.Instance.animationController.PlayGetCaughtAnimation();
                guard.CatchFrau();
                
            }
        }
    }

    

    public void DisableLight() {
        polygonCollider.enabled = false;
    }

    public void EnableLight()
    {
        polygonCollider.enabled = true;
    }
}