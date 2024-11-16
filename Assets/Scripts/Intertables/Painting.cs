using Intertables;

public class Painting : PickUpObjects
{
    private void Start()
    {
        Storage.addPainting();
    }

    public override void Interact()
    {
        base.Interact();
        CharacterController2D.Instance.SetSpeed(10f);
    }
}
