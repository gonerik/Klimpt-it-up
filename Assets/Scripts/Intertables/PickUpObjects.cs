using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intertables
{
    public class PickUpObjects : Interactable
    {
        public override void Interact()
        {
            base.Interact();
            // Assign the player as the target

            // Optionally disable collider to avoid further interactions
            GetComponent<Collider2D>().enabled = false;
            this.gameObject.layer = 0;
            Debug.Log("Picked up by player!");
        }
    }
}