using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour, IInteractable
{
    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public void Interact()
    {
        if (!playerInRange) return;

        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.OpenShop();
            Debug.Log("Tienda abierta");
        }
        else
        {
            Debug.LogError("ShopManager.Instance es NULL");
        }
    }
}


