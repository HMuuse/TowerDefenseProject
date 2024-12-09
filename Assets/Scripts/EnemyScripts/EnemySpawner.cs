using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();


    void Start()
    {
        GameManager.Instance.OnNewWave += GameManager_OnNewWave;
    }

    private void GameManager_OnNewWave(object sender, int waveCount)
    {
        int randomSpawnPoint = Random.Range(0, spawnPoints.Count);

        foreach (GameObject enemy in enemyPrefabs)
        {
            Instantiate(enemy, spawnPoints[randomSpawnPoint].position, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
