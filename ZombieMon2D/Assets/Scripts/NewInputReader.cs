using UnityEngine;

public class NewInputReader : MonoBehaviour, IInputReader
{
    private PlayerInputActions inputActions;
    private float horizontal;
    private bool interactPressed;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += ctx => horizontal = ctx.ReadValue<float>();
        inputActions.Player.Move.canceled += ctx => horizontal = 0f;

        inputActions.Player.Interact.performed += ctx => interactPressed = true;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public float GetHorizontal()
    {
        return horizontal;
    }

    public bool InteractPressed()
    {
        if (interactPressed)
        {
            interactPressed = false;
            return true;
        }
        return false;
    }
}
