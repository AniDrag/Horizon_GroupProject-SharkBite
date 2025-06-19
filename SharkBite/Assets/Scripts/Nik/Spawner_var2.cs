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

    // ========= Ints ==========
    private int currentWaveIndex = 0;

}

