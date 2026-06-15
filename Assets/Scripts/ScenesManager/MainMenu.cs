using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("level1");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
