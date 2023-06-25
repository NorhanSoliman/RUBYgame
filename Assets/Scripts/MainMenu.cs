using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        // Gets the current scene in stack and adds one to it to move to the next level
    }

    public void QuitGame()
    {
        //Application.Quit();
        Application.Quit();
    }
}
