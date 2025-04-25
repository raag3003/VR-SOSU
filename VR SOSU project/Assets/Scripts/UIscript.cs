using UnityEngine;
using UnityEngine.SceneManagement;

public class UIscript : MonoBehaviour
{
    
    // Leaves back to the main menu
    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

    // quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Loads the first scene (level 1)
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
