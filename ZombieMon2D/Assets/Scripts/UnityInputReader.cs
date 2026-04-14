using UnityEngine;

public class UnityInputReader : MonoBehaviour, IInputReader
{
    public float GetHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public bool InteractPressed()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
