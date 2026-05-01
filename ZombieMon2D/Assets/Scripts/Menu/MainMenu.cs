using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Game";

    [Header("UI")]
    [SerializeField] private GameObject creditsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }



    public void OpenOptions()
    {
        Debug.Log("Abrir opciones");
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();

        // Esto es SOLO para el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}