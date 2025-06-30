using UnityEngine;
using System.Collections.Generic;

public class LevelGenSettings : MonoBehaviour
{

}
[System.Serializable]
public class ObstacleDetails
{
    [Tooltip("Who am I?")]
    public GameObject prefab;

    [Tooltip("How much space do I take from my center?")]
    public Vector2 volume;

    [Tooltip("What is my minimum and maximum scale?")]
    [MinMaxSlider(0.1f, 10f)] public Vector2 scale;

    [Tooltip("What is my chance to spawn?")]
    [Min(1)] public int spawnChance;

    [Tooltip("What is my chance of being a group?")]
    [Range(0,100)] public float groupChance;

    [Tooltip("How many of me should be spawned in a group?")]
    [MinMaxSlider(1, 100)] public Vector2Int groupAmount;

    [Tooltip("How many of me should be spawned")]
    [MinMaxSlider(1, 100)] public Vector2Int amountChance;

    [Tooltip("How high should I spawn, if my scale is 1?")]
    public float yOffset;

    [Tooltip("How strong should I go up, when I'm bigger?")]
    [Min(0)] public float yToScale;

    //[Tooltip("What other prefabs should I ignore?")]
    //public List<GameObject> ignoreObstacles = new List<GameObject>();

}
