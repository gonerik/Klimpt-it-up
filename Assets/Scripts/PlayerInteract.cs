using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // How close you need to be to interact
    public LayerMask interactableLayer; // Set this to the layer of interactable objects
    public KeyCode interactionKey = KeyCode.E; // Key to press for interaction

    private Interactable currentInteractable;

    void Update()
    {
        DetectInteractable();

        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        // Cast a small sphere to detect objects within interaction range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

        if (hits.Length > 0)
        {
            // Assume the first hit is the closest interactable object
            currentInteractable = hits[0].GetComponent<Interactable>();
        }
        else
        {
            currentInteractable = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the interaction range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

