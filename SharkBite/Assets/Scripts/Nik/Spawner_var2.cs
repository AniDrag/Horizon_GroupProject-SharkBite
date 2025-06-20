using System.Collections.Generic;
using UnityEngine;
using static WaveFormatEditor;

public class Spawner_var2 : MonoBehaviour
{
    public static Spawner_var2 instance;

    [Header("========= Wave runner Logic =========")]
    [Tooltip("List of waves for the game")]
    public List<WaveFormat> waveList = new List<WaveFormat>();
    [Tooltip("Maximum number of enemies that can spawn at a time")]
    public int maxEnemiesToSpawn = 10;
    [Tooltip("Spawn interval between enemies")]
    public float spawnInterval = 1f;  // In seconds
    public float enemySpawnRadious = 10;

    // ========= Ints ==========
    private int currentWaveIndex = 0;
    private List<GameObject> _enemiesOnScreen = new List<GameObject>();

    private void FixedUpdate()
    {
        if (_enemiesOnScreen.Count < maxEnemiesToSpawn && currentWaveIndex < waveList.Count)
        {
            spawnInterval -= Time.deltaTime;
            if (spawnInterval < 0)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy(bool spawnBoss = false)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);

        Vector3 rndPos = new Vector3(GameManager.instance._playerPos.x + enemySpawnRadious * Mathf.Cos(angle), 0, GameManager.instance._playerPos.z + enemySpawnRadious * Mathf.Sin(angle));

        GameObject enemy = GetEnemyInGroup(currentWaveIndex);
        //Debug.Log(randomIndex);
        // if spawn boss is true it will spawn a boss else normal random enemy
        enemy = Instantiate(enemy, rndPos, Quaternion.identity);

        _enemiesOnScreen.Add(enemy);

    }
    GameObject GetEnemyInGroup( int waveIndex)
    {
        int totalWeight = 0;
        for (int i = 0; i < waveList[waveIndex].possibleEnemiesToSpawn.Count; i++)
        {
            totalWeight += waveList[waveIndex].possibleEnemiesToSpawn[i].SpawnChance;
        }

        // geting random vaulue in range of wight hat we have chosen
        int targetWeight = Random.Range(0, totalWeight);
        // is increased and compared to target weight
        int currentWeight = 0;

        for (int i = 0; i < waveList[waveIndex].possibleEnemiesToSpawn.Count; i++) { 
            currentWeight += waveList[waveIndex].possibleEnemiesToSpawn[i].SpawnChance;
            if (targetWeight < currentWeight)
                return waveList[waveIndex].possibleEnemiesToSpawn[i].prefab.prefab;
        }
        return null;
    }
}

