using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnStateChanged;

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField]
    private Canvas pauseCanvas;
    [SerializeField]
    private Canvas playCanvas;

    private Dictionary<GameState, Canvas> canvasDictionary;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    [SerializeField]
    private GameState currentState;
    // Read only access to private currentState field
    public GameState CurrentState => currentState;

    [SerializeField]
    private int score;
    [SerializeField]
    private int lives;
    private float waveTimer;
    [SerializeField]
    private float waveInterval = 15f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager instances found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        InitializeGame();
    }

    private void Start()
    {
        // Initialize canvas dictionary
        canvasDictionary = new Dictionary<GameState, Canvas>
        {
            { GameState.Playing, playCanvas },
            { GameState.Paused, pauseCanvas },
            { GameState.GameOver, gameOverCanvas }
        };

        // Ensure all canvases are disabled except for the initial state
        foreach (var canvas in canvasDictionary.Values)
        {
            if (canvas != null)
                canvas.gameObject.SetActive(false);
        }

        // Activate the current state's canvas
        ActivateCanvas(currentState);
    }

    private void Update()
    {
        if (currentState == GameState.GameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Set the current state to Playing or Paused depending on the current state
            SetGameState(currentState == GameState.Playing ? GameState.Paused : GameState.Playing);
        }

        if (currentState == GameState.Playing)
        {
            HandleWaveTimer();
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;
        HandleGameStateChanges(newState);

        OnStateChanged?.Invoke(newState);
    }

    private void HandleGameStateChanges(GameState newState)
    {
        Debug.Log($"Game state changed to: {newState}");
        ActivateCanvas(newState);

        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Debug.Log("Game Over!");
                break;
        }
    }

    private void ActivateCanvas(GameState state)
    {
        // Deactivate all canvases
        foreach (Canvas canvas in canvasDictionary.Values)
        {
            if (canvas != null)
                canvas.gameObject.SetActive(false);
        }

        // Activate the canvas corresponding to the current state
        if (canvasDictionary.TryGetValue(state, out Canvas activeCanvas) && activeCanvas != null)
        {
            activeCanvas.gameObject.SetActive(true);
        }
    }

    private void HandleWaveTimer()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInterval)
        {
            SpawnEnemyWave();
            waveTimer = 0f;
        }
    }

    private void SpawnEnemyWave()
    {
        // TODO: spawn enemies
    }

    private void InitializeGame()
    {
        score = 0;
        lives = 3;
        waveTimer = 0f;
        SetGameState(GameState.Playing);
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public int GetScore()
    {
        return score;
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            SetGameState(GameState.GameOver);
        }
    }
    public int GetLives()
    {
        return lives;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
