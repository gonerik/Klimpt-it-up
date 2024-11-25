using System;
using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Storage : Interactable
{
    private static int paintingsCount = 0;
    private int paintingsCollected = 0;
    public static Action OnStorageFull;
    private Transform particle;
    
    public void Awake()
    {
        paintingsCount = 0;
        
    }

    public void Start()
    {
        particle = GetComponentInChildren<Transform>();
    }

    public override void Interact()
    {
        base.Interact();
        if (CharacterController2D.Instance.currentPickup != null
            && CharacterController2D.Instance.currentPickup is Painting)
        {
            paintingsCollected++;
            CharacterController2D.Instance.settoMaxSpeed();
            if (paintingsCollected == paintingsCount)
            {
                CharacterController2D.Instance.animationController.PlayLevelCompletionAnimation();
                CharacterController2D.Instance.setCanMove(false);
                foreach (var i in particle.GetComponentsInChildren<ParticleSystem>())
                {
                    i.Stop();
                }
            }
            // Make the painting disappear
            CharacterController2D.Instance.currentPickup.gameObject.SetActive(false);
            CharacterController2D.Instance.currentPickup = null;
            CharacterController2D.Instance.SetIsHoldingPickUpObject(false);
        }
    }

    public static void addPainting()
    {
        paintingsCount++;
        Debug.Log(paintingsCount);
    }
}