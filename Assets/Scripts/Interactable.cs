using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool pickable = false;
    

    public virtual void Interact()
    {
        Debug.Log("Interact");
    }
}
