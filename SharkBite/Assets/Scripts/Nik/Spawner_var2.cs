using System.Collections.Generic;
using UnityEngine;

public class Spawner_var2 : MonoBehaviour
{
    public static Spawner_var2 instance;

    [SerializeField] private GameObject BossPrefab;
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

    public List<GameObject> _enemiesOnScreen = new List<GameObject>();
    private Pooler _pooler;
    private GameManager _GM;
    private int _spawnCount;
    private float _waveInterval;
    private float _lastSpawnTime;
    private float _lastWaveTime;

    private bool _isBossWave;
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
        _GM = GameManager.instance;
        if (_GM == null)
        {
            Debug.LogWarning("GameManager instance missing.");
            return;
        }
        _pooler = Pooler.instance;
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

        Vector3 playerPos = _GM._playerPos;

        float angle = Random.Range(0, Mathf.PI * 2);

        Vector3 rndPos = new Vector3(
            playerPos.x + enemySpawnRadius * Mathf.Cos(angle),
            0,
            playerPos.z + enemySpawnRadius * Mathf.Sin(angle)
        );
        GameObject enemyPrefab;
        if (!_isBossWave)
        {
            enemyPrefab = GetEnemyInGroup(currentWaveIndex, rndPos);
            if (enemyPrefab == null)
            {
                Debug.LogWarning("No enemy prefab found for this wave.");
                return;
            }

            if (_isRusherWave)
            {
                Vector3 direction = (playerPos - rndPos).normalized;
                if (direction != Vector3.zero)
                    enemyPrefab.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            enemyPrefab = Instantiate(BossPrefab, transform);
            enemyPrefab.transform.position = rndPos;
        }

        _enemiesOnScreen.Add(enemyPrefab);

    }
    GameObject GetEnemyInGroup( int waveIndex, Vector3 position)
    {
        if (waveIndex < 0 || waveIndex >= waveList.Count) return null;

        int totalWeight = 0;
        for (int i = 0; i < waveList[waveIndex].possibleEnemiesToSpawn.Count; i++)
        {
            totalWeight += waveList[waveIndex].possibleEnemiesToSpawn[i].SpawnChance;
        } //Total weight calculation

        int targetWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;

        int count = waveList[waveIndex].possibleEnemiesToSpawn.Count;

        for (int i = 0; i < count; i++) { 

            currentWeight += waveList[waveIndex].possibleEnemiesToSpawn[i].SpawnChance;
           

            if (targetWeight < currentWeight)
            {
                GameObject obj = _pooler.SpawnFromPool(waveList[waveIndex].possibleEnemiesToSpawn[i].prefab.enemyName, position, Quaternion.identity);
                if (obj == null)Debug.LogWarning("There is no fucking object found");
                if (!obj.GetComponent<EnemyCore>() || !obj.GetComponent<EnemyHealth_SYS>() || !obj.GetComponent<EnemyMovement>()) Debug.LogWarning("No flipn scrits found.. god damn it");

                return obj;
            }
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
        _enemiesOnScreen.Clear();// remove list of enemies so new enemies can spawn

        //_lastSpawnTime = Time.time;
        _lastWaveTime = Time.time;
        _waveInterval = wave.durationOfWave;
        _isRestWave = wave.isRestWave;
        _isRusherWave = wave.isRusherWave;
        _isBossWave = wave.isBossWave;
        spawnInterval = wave.spawnRate;
        _spawnCount = _isRusherWave ? squadEnemiesToSpawn : maxEnemiesToSpawn;
        _spawnCount = _isBossWave ? 1 : maxEnemiesToSpawn; // mybe set to 2 if needed
        _updatingWave = false;
    }
}

