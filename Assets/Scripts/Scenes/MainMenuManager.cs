using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level 2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}