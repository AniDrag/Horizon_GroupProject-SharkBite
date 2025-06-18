using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    #region Variables 
    [Header("========= Settings =========")]
    [Tooltip("This is in minutes")]
    [SerializeField] private int timeBeforeBossSpawn_min = 4;
    [Tooltip("This spawns normal enemysPrefab, and is a timer for that in seconds")]
    [SerializeField] private float enemySpawnInterval = 0.1f;
    [Tooltip("This are the enemysPrefab that always chase you")]
    [SerializeField] private int maxChaseEnemys = 10;
    [Tooltip("X = min amount, Y = Max amount")]
    public Vector2Int SPAWN_enemyPlatonSpawnMinMax = new Vector2Int(5, 20);
    [Tooltip("the distance from witch the enemysPrefab will spawn from the player")]
    [SerializeField] private float enemySpawnRadious = 10;

    [Header("========= Refrences =========")]
    [SerializeField] private List<GameObject> enemysPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private TMP_Text timeText;

    [Header("========= Enemies overview =========")]
    [Tooltip("A collection of all the enemies in the scene")]
    public List<GameObject> SPAWN_enemysInScene = new List<GameObject>();


    //---------------- Floats ----------------------------------
    [Tooltip("Complete game time")]
    private float _gameTime;
    [Tooltip("Time before a new enemy can spawn")]
    private float _spawnIntervalTime;


    //---------------- Ints ----------------------------------


    //---------------- Bools ----------------------------------
    public bool SPAWN_bossSpawned;

    //---------------- Vectors ----------------------------------
    [Tooltip("The position of the player in the world")]
    private Vector3 _playerPosition;


    #endregion[Tooltip("All variables and stiff")]

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        SafetyCheck();
    }
    void OnDestroy()
    {
        // Remove this object from the list if it exists
        if (SPAWN_enemysInScene.Contains(gameObject))
        {
            SPAWN_enemysInScene.Remove(gameObject);
            Debug.Log(gameObject.name + " removed from the list.");
        }
    }

    private void FixedUpdate()
    {
        timeText.text = Time.timeSinceLevelLoad.ToString();
        _playerPosition = GameManager.instance._playerPos;


        if (SPAWN_enemysInScene.Count < maxChaseEnemys && Time.timeSinceLevelLoad < (timeBeforeBossSpawn_min * 60))
        {
            _spawnIntervalTime -= Time.deltaTime;
            if (_spawnIntervalTime < 0)
            {
                SpawnEnemy();
            }
        }
        else if (!SPAWN_bossSpawned && Time.timeSinceLevelLoad > (timeBeforeBossSpawn_min * 60))
        {
            SPAWN_bossSpawned = true;
            if (SPAWN_enemysInScene.Count > 0)
            {
                for (int i = 0; i <= SPAWN_enemysInScene.Count - 1; i++)
                {
                    Destroy(SPAWN_enemysInScene[i]);
                }
            }
            SPAWN_enemysInScene.Clear();
            SpawnEnemy(true);            
        }
    }
   
    void SpawnEnemy(bool spawnBoss = false)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);

        Vector3 rndPos = new Vector3(_playerPosition.x + enemySpawnRadious * Mathf.Cos(angle), 0, _playerPosition.z + enemySpawnRadious * Mathf.Sin(angle));

        GameObject enemy;
        int randomIndex = Random.Range(0, enemysPrefab.Count);
        //Debug.Log(randomIndex);
        // if spawn boss is true it will spawn a boss else normal random enemy
        enemy = Instantiate(spawnBoss ? bossPrefab:enemysPrefab[randomIndex], rndPos, Quaternion.identity);
        
        SPAWN_enemysInScene.Add(enemy);
        
        _spawnIntervalTime = enemySpawnInterval;

    }
    #region Public functions
    public void SetSpawnSettings(int minCount, int maxCount)
    {
        SPAWN_enemyPlatonSpawnMinMax = new Vector2Int(minCount, maxCount);
    }

    #endregion

    void SafetyCheck()
    {
        if (enemysPrefab.Count <= 0) Debug.LogError("No enemy prefabs on Spawner") ;
        if (timeBeforeBossSpawn_min <= 0) Debug.LogError("timeBeforeBossSpawn_min = 0 or less, This can't be negative !!");
        if (enemySpawnInterval <= 0) Debug.LogError("enemySpawnInterval = 0 or less, This can't be negative !!");
        if (maxChaseEnemys <= 0) Debug.LogError("maxChaseEnemys = 0 or less, NO enemies will spawn Fix");
        if (SPAWN_enemyPlatonSpawnMinMax.x <= 0) Debug.LogError("SPAWN_enemyPlatonSpawnMinMax value X is 0 or less, This cant be negative or 0 !!");
        if (SPAWN_enemyPlatonSpawnMinMax.y <= 0) Debug.LogError("SPAWN_enemyPlatonSpawnMinMax value X is 0 or less, This cant be negative or 0 !!");
        if (SPAWN_enemyPlatonSpawnMinMax.x >= SPAWN_enemyPlatonSpawnMinMax.y) Debug.LogError("SPAWN_enemyPlatonSpawnMinMax error, X(min) cant be bigger than Y(max), causes no real issue but fix it for cleanlines");
        SPAWN_bossSpawned = false;
        Debug.Log("Spawner Clear check :: Passed");
    }
}
