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
        CharacterController2D.Instance.animationController.DisableEmojiCamera();
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadSceneAsync(0);
    }

    public void resertLevel()
    {
        CharacterController2D.Instance.animationController.DisableEmojiCamera();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void holdPainting()
    {
        CharacterController2D.Instance.setCanMove(true);
        CharacterController2D.Instance.SetIsHoldingPickUpObject(true);
    }
}
