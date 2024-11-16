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
        paintingsCollected++;
        CharacterController2D.Instance.settoMaxSpeed();
        Debug.Log(paintingsCollected);
        Debug.Log(paintingsCount);
        
        if (paintingsCollected == paintingsCount)
        {
            StartCoroutine(GoToNextLevel());
        }
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
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadSceneAsync(0);
    }
}