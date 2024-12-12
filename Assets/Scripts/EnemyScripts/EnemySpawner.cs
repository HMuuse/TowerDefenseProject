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
        int enemiesToSpawn = enemyPrefabs.Count * waveCount;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Count);
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);

            GameObject enemy = 
                Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
            if (enemyScript != null)
            {
                enemyScript.health = enemyScript.health * waveCount;
            }
        }

    }

    void Update()
    {
        
    }
}
