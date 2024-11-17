using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    private void Awake()
    {
        // If an instance already exists, destroy this duplicate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Make this the singleton instance
        instance = this;
        DontDestroyOnLoad(gameObject); // Prevent destruction on scene load
    }

    private void Start()
    {
        // Start playing music if not already playing
        AudioSource audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}