using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameStates
{
    countDown,
    running,
    raceOver
}

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance = null;
    GameStates gameState = GameStates.countDown;

    float raceStartTime = 0;
    float raceFinishTime = 0;

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        SceneManager.LoadScene("Title Screen");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }

    public void LevelStart()
    {
        gameState = GameStates.countDown;
        Debug.Log("Level Start");
    }

    public void OnRaceStart()
    {
        Debug.Log("Race Start");
        raceStartTime = Time.time;
        ChangeGameState(GameStates.running);
    }

    public void OnRaceOver()
    {
        Debug.Log("Race Over");
        raceFinishTime = Time.time;
        ChangeGameState(GameStates.raceOver);
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    public void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
        {
            return 0;
        }
        if (gameState == GameStates.raceOver)
        {
            return raceFinishTime - raceStartTime;
        }
        else
        {
            return Time.time - raceStartTime;
        }
    }
}
