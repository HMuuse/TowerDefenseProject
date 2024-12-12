using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour, IRegisterEnemy
{
    public event Action<GameState> OnStateChanged;
    public event EventHandler<int> OnNewWave;
    public event EventHandler<int> OnScoreChanged;

    public static GameManager Instance { get; private set; }

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
    private float waveInterval;
    [SerializeField]
    private int currentWave = 1;

    [SerializeField]
    private List<BaseEnemy> enemyList = new List<BaseEnemy>();

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
    }

    private void Start()
    {
        InitializeGame();
        OnNewWave?.Invoke(this, currentWave);
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

    private void HandleWaveTimer()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInterval)
        {
            currentWave++;
            OnNewWave?.Invoke(this, currentWave);
            waveTimer = 0f;
        }
    }

    private void InitializeGame()
    {
        lives = 3;
        waveTimer = 0f;
        SetGameState(GameState.Playing);
    }

    public void AddScore(int points)
    {
        score += points;
        OnScoreChanged?.Invoke(this, score);
    }

    public void RemoveScore(int points)
    {
        score -= points;
        OnScoreChanged?.Invoke(this, score);
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

    private void BaseEnemy_OnEnemyDied(object sender, int scoreToAdd)
    {
        AddScore(scoreToAdd);
    }

    public void RegisterEnemy(BaseEnemy enemy)
    {
        if (!enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
            enemy.OnEnemyDied += BaseEnemy_OnEnemyDied;
        }
    }

    public void DeregisterEnemy(BaseEnemy enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            enemy.OnEnemyDied -= BaseEnemy_OnEnemyDied;
        }
    }
}
