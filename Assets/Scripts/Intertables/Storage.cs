using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Painting stored in the storage!");
    }
}