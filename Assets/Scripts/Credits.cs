using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image visual;
    private int currentSprite = 0;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        visual.gameObject.GetComponent<Animator>().Play("VisualForCredits");
        animator.Play("CreditsAnimation");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }

    // Update is called once per frame
    public void LoadMainMenu()
    {
        if (BackgroundMusic.instance != null)
        {
            BackgroundMusic.instance.destroy();
        }
        SceneManager.LoadSceneAsync(0);
    }

    public void changeSprite()
    {
        visual.sprite = sprites[currentSprite];
        currentSprite = currentSprite + 1;
    }
}
