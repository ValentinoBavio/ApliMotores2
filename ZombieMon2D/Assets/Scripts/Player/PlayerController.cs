using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IInputReader input;
    private IMovement movement;

    private InteractionDetector interaction;

    private void Awake()
    {
        input = GetComponent<IInputReader>();
        movement = GetComponent<IMovement>();
        interaction = GetComponent<InteractionDetector>();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        if (GameManager.Instance.CurrentState != GameState.Exploration)
            return;

        float dir = input.GetHorizontal();
        movement.Move(dir);
    }

    private void HandleInteraction()
    {
        if (input.InteractPressed())
        {
            interaction.TryInteract();
            Debug.Log("interactue");
        }
    }

}


