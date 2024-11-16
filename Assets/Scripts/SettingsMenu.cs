using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private bool _paused = false;
    public static SettingsMenu instance;
    
    private void Start()
    {
        gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
            Debug.LogError("More than one SettingsMenu in scene!");
        }
        
    }
    [SerializeField] private AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    

    public void exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        _paused = !_paused;
        // Time.timeScale = (!_paused ? 1 : 0);
        gameObject.SetActive(!gameObject.activeSelf);
    }
}