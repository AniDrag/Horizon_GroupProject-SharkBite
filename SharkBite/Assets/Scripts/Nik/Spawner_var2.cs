using System.Collections.Generic;
using UnityEngine;

public class Spawner_var2 : MonoBehaviour
{
    public static Spawner_var2 instance;

    [SerializeField] private int currentWaveIndex = 0;

    [Header("========= Wave runner Logic =========")]
    [Tooltip("List of waves for the game")]
    public List<WaveFormat> waveList;

    [Tooltip("Maximum number of enemies that can spawn at a time")]
    [SerializeField] private int maxEnemiesToSpawn = 10;

    [SerializeField] private int squadEnemiesToSpawn = 3;

    [Tooltip("Spawn interval between enemies")]
    [SerializeField] private float spawnInterval = 1f;

    [SerializeField] private float enemySpawnRadius = 10f;

    private List<GameObject> _enemiesOnScreen = new List<GameObject>();
    private int _spawnCount;
    private float _waveInterval;
    private float _lastSpawnTime;
    private float _lastWaveTime;

    private bool _isRestWave;
    private bool _isRusherWave;
    private bool _updatingWave;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);

        }
        instance = this;    
    }
    private void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogWarning("GameManager instance missing.");
            return;
        }
        BaseCheck();
    }
    private void Update()
    {
        if (Time.time >= _lastWaveTime + _waveInterval && !_updatingWave && currentWaveIndex < waveList.Count - 1)
        {
            _updatingWave = true;
            currentWaveIndex += 1;
            Debug.Log("Updating Wave");
            BaseCheck();
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

        Vector3 playerPos = GameManager.instance._playerPos;

        float angle = Random.Range(0, Mathf.PI * 2);

        Vector3 rndPos = new Vector3(
            playerPos.x + enemySpawnRadius * Mathf.Cos(angle),
            0,
            playerPos.z + enemySpawnRadius * Mathf.Sin(angle)
        );

        GameObject enemyPrefab = GetEnemyInGroup(currentWaveIndex);
        if (enemyPrefab == null)
        {
            Debug.LogWarning("No enemy prefab found for this wave.");
            return;
        }

        // Recommended: Replace with pooled version
        GameObject enemy = Instantiate(enemyPrefab, rndPos, Quaternion.identity);

        if (_isRusherWave)
        {
            Vector3 direction = (playerPos - rndPos).normalized;
            if (direction != Vector3.zero)
                enemy.transform.rotation = Quaternion.LookRotation(direction);
        }

        _enemiesOnScreen.Add(enemy);

    }
    GameObject GetEnemyInGroup( int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveList.Count)
            return null;

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

    void BaseCheck()
    {
        if (currentWaveIndex < 0 || currentWaveIndex >= waveList.Count)
        {
            Debug.LogWarning("Invalid wave index.");
            return;
        }

        var wave = waveList[currentWaveIndex];

        //_lastSpawnTime = Time.time;
        _lastWaveTime = Time.time;
        _waveInterval = wave.durationOfWave;
        _isRestWave = wave.isRestWave;
        _isRusherWave = wave.isRusherWave;
        _spawnCount = _isRusherWave ? squadEnemiesToSpawn : maxEnemiesToSpawn;
        _updatingWave = false;
    }
}

