using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable currentInteractable;

    [Header("UI")]
    [SerializeField] private GameObject battleButton;

    private void Start()
    {
        if (battleButton != null)
            battleButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entre en trigger: " + collision.name);

        currentInteractable = collision.GetComponent<IInteractable>();

        if (collision.GetComponent<BattleTrigger>() != null)
        {
            battleButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() == currentInteractable)
            currentInteractable = null;

        if (collision.GetComponent<BattleTrigger>() != null)
        {
            battleButton.SetActive(false);
        }
    }

    public void TryInteract()
    {
        currentInteractable?.Interact();
    }
}