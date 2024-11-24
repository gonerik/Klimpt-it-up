using Intertables;
using UnityEngine;

public class Painting : PickUpObjects
{
    private ParticleSystem[] particleSystem;
    private void Start()
    {
        Storage.addPainting();
        particleSystem = GetComponentsInChildren<ParticleSystem>();
    }

    public override void Interact()
    {
        base.Interact();
        foreach (var i in particleSystem)
        {
            i.Stop();
        }
        CharacterController2D.Instance.settoMinSpeed();
    }
}
