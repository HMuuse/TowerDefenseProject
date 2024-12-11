using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private TextMeshProUGUI woodText;
    [SerializeField]
    private TextMeshProUGUI stoneText;

    private void Start()
    {
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
        ResourceManager.Instance.OnGoldChanged += ResourceManager_OnGoldChanged;
        ResourceManager.Instance.OnStoneChanged += ResourceManager_OnStoneChanged;
        ResourceManager.Instance.OnWoodChanged += ResourceManager_OnWoodChanged;
    }

    private void ResourceManager_OnWoodChanged(object sender, int woodAmount)
    {
        woodText.text = woodAmount.ToString();
    }

    private void ResourceManager_OnStoneChanged(object sender, int stoneAmount)
    {
        stoneText.text = stoneAmount.ToString();
    }

    private void ResourceManager_OnGoldChanged(object sender, int goldAmount)
    {
        goldText.text = goldAmount.ToString();
    }

    private void GameManager_OnScoreChanged(object sender, int score)
    {
        scoreText.text = "Score: " + score;
    }
}
