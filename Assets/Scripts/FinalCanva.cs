using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalCanva : MonoBehaviour
{
    public static FinalCanva Instance;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        SettingsMenu.instance.PlayClip(2);
    }

    public void Display(PaintingFinal painting)
    {
        text.text = painting.Description;
        image.sprite = painting.Sprite;
        title.text = painting.Title;
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("Start");
    }
}

