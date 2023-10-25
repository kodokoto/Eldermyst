using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro.EditorUtilities;
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
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        gameState = GameState.Playing;
    }

    private void Update()
    {
        Debug.Log("Game state: " + gameState);
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

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // restart player 
    }

    public void MainMenu()
    {
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
