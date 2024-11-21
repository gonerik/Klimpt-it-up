using Intertables;
using UnityEngine;

public class Painting : PickUpObjects
{
    private ParticleSystem particleSystem;
    private void Start()
    {
        Storage.addPainting();
        particleSystem = GetComponent<ParticleSystem>();
    }

    public override void Interact()
    {
        base.Interact();
        particleSystem.Stop();
        CharacterController2D.Instance.settoMinSpeed();
    }
}
