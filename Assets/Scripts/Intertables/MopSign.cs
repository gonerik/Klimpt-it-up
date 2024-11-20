using System;
using System.Collections;
using Intertables;
using UnityEngine;
using UnityEngine.Events;

public class MopSign : PickUpObjects
{
    public static event EventHandler OnPickUp;
    public override void Interact()
    {
        base.Interact();
        // Assign the player as the target
        OnPickUp?.Invoke(this, EventArgs.Empty);
        // Optionally disable collider to avoid further interactions
        GetComponent<Collider2D>().enabled = false;
        CharacterController2D.Instance.settoMinSpeed();
    }
}
