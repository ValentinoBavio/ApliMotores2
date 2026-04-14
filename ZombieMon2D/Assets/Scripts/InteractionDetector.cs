using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable currentInteractable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entre en trigger: " + collision.name);

        currentInteractable = collision.GetComponent<IInteractable>();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() == currentInteractable)
            currentInteractable = null;
    }

    public void TryInteract()
    {
        currentInteractable?.Interact();
    }
}
