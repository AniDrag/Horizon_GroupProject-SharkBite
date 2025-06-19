using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public class WaveFormat
    {
        public int durationOfWave = 0;
        public List<GameObject> enemiesInWave = new List<GameObject>();
        public bool isRestWave;
    }
    public static Spawner instance;

    #region Variables 
    [Header("========= Settings =========")]
    [Tooltip("This is in minutes")]
    [SerializeField, Min(1)]  private int timeBeforeBossSpawn_min = 4;
    [Tooltip("This spawns normal enemysPrefab, and is a timer for that in seconds")]
    [SerializeField, Min(0.01f)] private float enemySpawnInterval = 0.1f;
    [Tooltip("This are the enemysPrefab that always chase you")]
    [SerializeField, Min(1)] private int maxChaseEnemys = 10;
    [Tooltip("X = min amount, Y = Max amount")]
    [MinMaxSlider(1f, 100f)] public Vector2Int SPAWN_enemyPlatonSpawnMinMax = new Vector2Int(5, 20);
    [Tooltip("the distance from witch the enemysPrefab will spawn from the player")]
    [SerializeField, Min(10)] private float enemySpawnRadious = 10;

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
        SPAWN_bossSpawned = false;
        Debug.Log("Spawner Clear check :: Passed");
    }
}
