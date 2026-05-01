using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable currentInteractable;

    [Header("UI")]
    [SerializeField] private GameObject interactionButton;

    private void Start()
    {
        if (interactionButton != null)
            interactionButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;

            if (interactionButton != null)
                interactionButton.SetActive(true);

            Debug.Log("Interactuable detectado: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;

            if (interactionButton != null)
                interactionButton.SetActive(false);

            Debug.Log("Salí del interactuable: " + collision.name);
        }
    }

    public void TryInteract()
    {
        currentInteractable?.Interact();
    }
}