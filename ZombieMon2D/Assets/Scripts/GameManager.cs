using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetState(GameState.Exploration);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }
}
