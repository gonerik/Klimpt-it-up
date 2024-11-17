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
    public void Awake()
    {
        paintingsCount = 0;
        
    }

    public override void Interact()
    {
        base.Interact();
        if (CharacterController2D.Instance.currentPickup != null
            && CharacterController2D.Instance.currentPickup is Painting)
        {
            paintingsCollected++;
            CharacterController2D.Instance.settoMaxSpeed();
            Debug.Log(paintingsCollected);
            Debug.Log(paintingsCount);
            if (paintingsCollected == paintingsCount)
            {
                StartCoroutine(GoToNextLevel());
            }
            // Make the painting disappear
            CharacterController2D.Instance.currentPickup.gameObject.SetActive(false);
            CharacterController2D.Instance.currentPickup = null;
            CharacterController2D.Instance.SetIsHoldingPickUpObject(false);
        }
        else Debug.Log("Cannot deposit mop sign");
    }

    public static void addPainting()
    {
        paintingsCount++;
        Debug.Log(paintingsCount);
    }

    public IEnumerator GoToNextLevel(){
        //Play win animation here
        CharacterController2D.Instance.setCanMove(false);
        Debug.Log(paintingsCollected);
        yield return new WaitForSeconds(5);
        Debug.Log(SceneManager.GetActiveScene().buildIndex +" : "+ SceneManager.sceneCountInBuildSettings);
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadSceneAsync(0);
    }
}