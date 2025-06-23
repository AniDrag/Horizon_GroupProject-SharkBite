using System.Collections.Generic;
using UnityEngine;

public class Spawner_var2 : MonoBehaviour
{
    public static Spawner_var2 instance;

    [SerializeField] private int currentWaveIndex = 0;
    [Header("========= Wave runner Logic =========")]
    [Tooltip("List of waves for the game")]
    public List<WaveFormat> waveList = new List<WaveFormat>();
    [Tooltip("Maximum number of enemies that can spawn at a time")]
    [SerializeField] private int maxEnemiesToSpawn = 10;
    [SerializeField] private int squadEnemiesToSpawn = 3;
    [Tooltip("Spawn interval between enemies")]
    [SerializeField]private float spawnInterval = 1f;  // In seconds
    [SerializeField] private float enemySpawnRadious = 10;

    // ========= Ints ==========
    private List<GameObject> _enemiesOnScreen = new List<GameObject>();
    private int _spawnCount;
    private float _waveTimer;
    private float _lastSpawnTime;
    private bool _isRestWave;
    private bool _isRusherWave;
    private bool _updatingWave;
    private void Start()
    {
        BaseChech();
    }
    private void Update()
    {
        if (Time.time >= _waveTimer + spawnInterval && !_updatingWave)
        {
            _updatingWave = true;
            currentWaveIndex += 1;
            Debug.Log("Updating Wave");
            BaseChech();
        }
    }
    private void FixedUpdate()
    {
        

        if (!_isRestWave && !_updatingWave)
        {
            if (Time.time >= _lastSpawnTime + spawnInterval && _enemiesOnScreen.Count < _spawnCount && currentWaveIndex < waveList.Count-1)
            {
                _lastSpawnTime = Time.time;
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy() { 
        float angle = Random.Range(0, 2 * Mathf.PI);

        Vector3 rndPos = new Vector3(GameManager.instance._playerPos.x + enemySpawnRadious * Mathf.Cos(angle), 0, GameManager.instance._playerPos.z + enemySpawnRadious * Mathf.Sin(angle));

        GameObject enemy = GetEnemyInGroup(currentWaveIndex);
        //Debug.Log(randomIndex);
        enemy = Instantiate(enemy, rndPos, Quaternion.identity);
        if (_isRusherWave)
        {
            enemy.transform.rotation = Quaternion.LookRotation(enemy.transform.position, GameManager.instance._playerPos);
        }
        

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

    void BaseChech()
    {
        _lastSpawnTime = Time.time;
        spawnInterval = waveList[currentWaveIndex].durationOfWave;
        _isRestWave = waveList[currentWaveIndex].isRestWave;
        _isRestWave = waveList[currentWaveIndex].isRusherWave;
        _spawnCount = _isRusherWave ? squadEnemiesToSpawn : maxEnemiesToSpawn;
        _updatingWave = false;
    }
}

