using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSystemUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waveText;

    private void Start()
    {
        GameManager.Instance.OnNewWave += GameManager_OnNewWave;
    }

    private void GameManager_OnNewWave(object sender, int waveCount)
    {
        waveText.text = "WAVE: " + waveCount;
    }
}
