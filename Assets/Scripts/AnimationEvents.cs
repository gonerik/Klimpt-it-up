using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvents : MonoBehaviour
{
    public void enablePlayerMovement()
    {
        Debug.Log("Enable player movement");
        CharacterController2D.Instance.setCanMove(true);
    }

    public void finishLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadSceneAsync(0);
    }
}
