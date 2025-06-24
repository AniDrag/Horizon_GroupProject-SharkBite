using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolData
    {
        [Tooltip("This represents the name of the pool")]
        public string ID;
        [Tooltip("Stores the item prefab")]
        public GameObject prefab;
        [Tooltip("The amount of prefabs that are rady in scene before reusing the prefabs")]
        public int size;
    }

    #region Singelton patern
    public static Pooler instance;
    private void Awake()
    {
        instance = this;        
    }
    #endregion

    public List<PoolData> poolDatas = new List<PoolData>();
    private Dictionary<string, Queue<GameObject>> _poolerDictionary;

    /// <summary>
    /// This spawns all objects needed for the game
    /// </summary>
    private void Start()
    {
        _poolerDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(PoolData pool in poolDatas)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab,transform);                
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            _poolerDictionary.Add(pool.ID, objectQueue);
        }
    }

    /// <summary>
    /// This gets an already inactive item. I is then spawnd and requed for reuse thus Instantiation isnt necesarry.
    /// </summary>
    /// <param name="ID">naem of item</param>
    /// <param name="position"> spawn position</param>
    /// <param name="rotation"> rotation </param>
    public GameObject SpawnFromPool(string ID, Vector3 position, Quaternion rotation)
    {
        if (!_poolerDictionary.ContainsKey(ID))
        {
            Debug.LogWarning($"!!! ID of item: !!{ID}!! is missing !!!");
            return null;
        }
        GameObject spawnable = _poolerDictionary[ID].Dequeue();

        spawnable.SetActive(true);
        spawnable.transform.position = position;
        spawnable.transform.rotation = rotation;

        // === Reset the object ===
        foreach (var pooledObject in spawnable.GetComponents<IPooledObject>())
        {
            pooledObject?.RespawndObject();
        }

        _poolerDictionary[ID].Enqueue(spawnable);
        return spawnable;
    }
}
