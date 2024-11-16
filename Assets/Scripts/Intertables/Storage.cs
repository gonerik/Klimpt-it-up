using System;
using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Storage : Interactable
{
    private static int levelCount = 1;
    private int paintingsCount = 0;
    private int paintingsCollected = 0;
    public void Start()
    {
        paintingsCount = 0;
        levelCount++;
    }

    public override void Interact()
    {
        base.Interact();
        paintingsCollected++;
        CharacterController2D.Instance.SetSpeed(CharacterController2D.Instance.maxRunSpeed);
        if (paintingsCollected == paintingsCount)
        {
            
        }
    }

    public void addPainting()
    {
        paintingsCount++;
    }

    public IEnumerator GoToNextLevel(){
        //Play win animation here
        CharacterController2D.Instance.setPlayerMovement(false);
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync(levelCount);
    }
}