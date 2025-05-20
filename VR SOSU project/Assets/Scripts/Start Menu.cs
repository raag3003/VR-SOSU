using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    /*public void StartGame()
    {
        SceneManager.LoadScene(1);
    }*/

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting the game");
    }

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }
    public void Level2()
    {
        SceneManager.LoadScene(2);
    }

    
}
