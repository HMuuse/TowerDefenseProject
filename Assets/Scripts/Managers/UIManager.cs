using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField]
    private Canvas pauseCanvas;
    [SerializeField]
    private Canvas playCanvas;

    private Dictionary<GameState, Canvas> canvasDictionary;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        // Initialize canvas dictionary
        canvasDictionary = new Dictionary<GameState, Canvas>
        {
            { GameManager.GameState.Playing, playCanvas },
            { GameManager.GameState.Paused, pauseCanvas },
            { GameManager.GameState.GameOver, gameOverCanvas }
        };



        // Ensure all canvases are disabled except for the initial state
        foreach (Canvas canvas in canvasDictionary.Values)
        {
            if (canvas != null)
                canvas.gameObject.SetActive(false);
        }

        // Activate the current state's canvas
        ActivateCanvas(GameManager.Instance.CurrentState);
    }

    private void GameManager_OnStateChanged(GameState newState)
    {
        ActivateCanvas(newState);
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
}
