using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerClasses : MonoBehaviour
{
    
}
[System.Serializable]
public class WaveFormat
{
    [Tooltip("Duration of the wave in seconds")]
    public float durationOfWave;

    [Tooltip("Whether this is a rest wave (no enemies will spawn)")]
    public bool isRestWave;

    [Tooltip("Whether this is a wave where groups of enemies spawn and rush the player")]
    public bool isRusherWave;

    public int chanceSum;

    [Tooltip("List of enemies that can be spawned during this wave")]
    public List<EnemySpecifics> possibleEnemiesToSpawn = new List<EnemySpecifics>();

    // Helper method to normalize spawn chances (to make sure total is 100%)
    public void NormalizeSpawnChances()
    {
        int totalChance = 0;

        // Calculate the total spawn chance for all enemies
        foreach (var enemy in possibleEnemiesToSpawn)
        {
            totalChance += enemy.SpawnChance;
        }

        // Adjust each enemy's spawn chance to fit within 100%
        foreach (var enemy in possibleEnemiesToSpawn)
        {
            enemy.SpawnChance = Mathf.FloorToInt((enemy.SpawnChance / (float)totalChance) * 100);
        }
    }
}

[System.Serializable]
public class EnemySpecifics
{
    [Tooltip("Insert scriptable onject of EnemyPrefab Here")]
    public EnemyPrefab prefab;

    [Tooltip("The name of the enemy")]
    public string EnemyName;

    [Tooltip("The enemy's image")]
    public Image EnemyImage;

    [Tooltip("The chance (percentage) that this enemy will spawn in this wave (0-100%)")]
    [Range(1, 100)]
    public int SpawnChance = 0;  // The chance this enemy will spawn (0-100%)

    public string GetEnemyName() => EnemyName = prefab.enemyName != null? prefab.enemyName: "No name";
    //public Sprite SetSprite() => EnemyImage = prefab.previewImage != null? prefab.previewImage:null;
}

