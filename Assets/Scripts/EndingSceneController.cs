using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingSceneController : MonoBehaviour
{
    public Button exitButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Assign button listeners
        exitButton.onClick.AddListener(ExitGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit(); // Closes the game
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}
