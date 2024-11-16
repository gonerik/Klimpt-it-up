using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool pickable = false;
    public void Interact()
    {
        Debug.Log("Interact");
    }
}
