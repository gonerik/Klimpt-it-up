using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool pickable = false;
    

    public virtual void Interact()
    {
    }
}
