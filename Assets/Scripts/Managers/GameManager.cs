using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Playing,
    Won,
    Lost
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameState = GameState.Playing;
    }

    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                HandleGamePlaying();
                break;
            case GameState.Won:
                HandleGameWon();
                break;
            case GameState.Lost:
                HandleGameLost();
                break;
        }
    }

    public void Retry()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        Destroy(gameObject);
        instance = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        // reset spawn point
        PlayerSpawnPoint.instance.SetSpawnPoint(Vector3.zero);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    // handlers

    private void HandleGameLost()
    {
        Debug.Log("Game lost");
        if (gameState == GameState.Playing)
        {
            Debug.Log("Game lost for real");
            gameState = GameState.Lost;
            UIManager.instance.ShowGameOverScreen();
        }
        else
        {
            Debug.LogWarning("Tried to set game state to lost when it is not playing");
        }
    }

    private void HandleGameWon()
    {
        if (gameState == GameState.Playing)
        {
            gameState = GameState.Won;
            UIManager.instance.ShowWinScreen();
        }
        else
        {
            Debug.LogWarning("Tried to set game state to won when it is not playing");
        }
    }

    private void HandleGamePlaying()
    {
        if (gameState == GameState.Won)
        {
            gameState = GameState.Playing;
            UIManager.instance.HideWinScreen();
        }
        else if (gameState == GameState.Lost)
        {
            gameState = GameState.Playing;
            UIManager.instance.HideGameOverScreen();
        }
        else
        {
            Debug.LogWarning("Tried to set game state to playing when it is already playing");
        }
    }

}
