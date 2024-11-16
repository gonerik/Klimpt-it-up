using System;
using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Storage : Interactable
{
    private static int levelCount = 1;
    private static int paintingsCount = 0;
    private int paintingsCollected = 0;
    public void Awake()
    {
        paintingsCount = 0;
        levelCount++;
        
    }

    public override void Interact()
    {
        base.Interact();
        paintingsCollected++;
        CharacterController2D.Instance.SetSpeed(CharacterController2D.Instance.maxRunSpeed);
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
        Debug.Log(paintingsCollected);
        CharacterController2D.Instance.setPlayerMovement(false);
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync(levelCount);
    }
}