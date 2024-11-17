using System.Collections;
using Intertables;
using UnityEngine;

public class MopSign : PickUpObjects
{
    public override void Interact()
    {
        base.Interact();
        // Assign the player as the target
        
        // Optionally disable collider to avoid further interactions
        GetComponent<Collider2D>().enabled = false;
        CharacterController2D.Instance.settoMinSpeed();
    }
}
