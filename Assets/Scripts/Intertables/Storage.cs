using Intertables;

public class Storage : Interactable
{
    public override void Interact()
    {
        base.Interact();
        CharacterController2D.Instance.SetSpeed(20f);
    }
}