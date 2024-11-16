using System;
using Intertables;

public class Painting : PickUpObjects
{
    public override void Interact()
    {
        base.Interact();
        CharacterController2D.Instance.SetSpeed(CharacterController2D.Instance.minRunSpeed);
    }
}
