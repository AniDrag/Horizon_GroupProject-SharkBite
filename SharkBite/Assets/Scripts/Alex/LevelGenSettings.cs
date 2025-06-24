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
    [Min(0)] public int groupChance;

    [Tooltip("What other prefabs should I ignore?")]
    public List<GameObject> ignoreObstacles = new List<GameObject>();

}
