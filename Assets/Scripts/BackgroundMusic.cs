using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    private void Awake()
    {
        // If an instance already exists, destroy this duplicate
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

    public void destroy()
    {
        Destroy(gameObject);
    }
}