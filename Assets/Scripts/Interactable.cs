using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionMessage = "Interactable";
    public void Interact()
    {
        Debug.Log("Interact");
    }
}
