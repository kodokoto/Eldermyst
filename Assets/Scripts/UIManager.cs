using UnityEngine;
using UnityEngine.UI;
// union type for screens

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowGameOverScreen()
    {
        gameObject.transform.Find("GameOverScreen").gameObject.SetActive(true);
    }

    public void HideGameOverScreen()
    {
        gameObject.transform.Find("GameOverScreen").gameObject.SetActive(false);
    }

    public void ShowWinScreen()
    {
        gameObject.transform.Find("WinScreen").gameObject.SetActive(true);
    }

    public void HideWinScreen()
    {
        gameObject.transform.Find("WinScreen").gameObject.SetActive(false);
    }
}