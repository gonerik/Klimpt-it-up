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
    private bool _paused;
    public static SettingsMenu instance;
    private Canvas _canvas;
    private AudioSource _audio;
    [SerializeField] private AudioClip[] audioClips;
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _canvas = GetComponentInChildren<Canvas>();
        _canvas.gameObject.SetActive(false);
        _audio = GetComponentInChildren<AudioSource>();

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

    public void ExitToMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayClip(0);
            SceneManager.LoadSceneAsync(0);
        }

        if (_paused)
        {
            Pause();
        }
    }

    public void Pause()
    {
        _paused = !_paused;
        // Time.timeScale = (!_paused ? 1 : 0);
        _canvas.gameObject.SetActive( !_canvas.gameObject.activeSelf);
    }

    public void Restart()
    {
        if (_paused)
        {
            Pause();
        }
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        
    }

    public void PlayClip(int clipIndex)
    {
        _audio.clip = audioClips[clipIndex];
        _audio.PlayDelayed(1);
    }
}