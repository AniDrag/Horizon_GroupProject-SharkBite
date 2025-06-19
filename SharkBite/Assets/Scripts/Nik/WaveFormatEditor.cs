using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.ComponentModel;

public class WaveFormatEditor : Editor
{
    private bool isAdjusted = false; // Flag to check if adjustments have been made

    public override void OnInspectorGUI()
    {
        Spawner_var2 spawner = (Spawner_var2)target;

        // Loop through each wave in the spawner
        foreach (var wave in spawner.waveList)
        {
            EditorGUILayout.LabelField("Wave Duration: " + wave.durationOfWave);
            EditorGUILayout.LabelField("Is Rest Wave: " + wave.isRestWave.ToString());

            // Ensure the total chance doesn't exceed 100%
            int totalChance = 0;
            foreach (var enemy in wave.possibleEnemiesToSpawn)
            {
                totalChance += enemy.SpawnChance;
            }

            // Show total spawn chance and warning if it's too high
            EditorGUILayout.LabelField("Total Spawn Chance: " + totalChance + "%");
            if (totalChance > 100)
            {
                EditorGUILayout.HelpBox("Total spawn chances exceed 100%. Adjust the values to make sure the sum is 100 or less.", MessageType.Warning);
            }


            // Display each enemy's name and spawn chance
            for (int i = 0; i < wave.possibleEnemiesToSpawn.Count; i++)
            {
                var enemy = wave.possibleEnemiesToSpawn[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(enemy.GetEnemyName(), GUILayout.Width(150));

                // Adjust spawn chance for this enemy
                int newChance = EditorGUILayout.IntSlider(enemy.SpawnChance, 0, 100);
                if (newChance != enemy.SpawnChance)
                {
                    // If the spawn chance changes, adjust the others automatically
                    AdjustOtherEnemiesSpawnChance(wave, i, newChance);
                    enemy.SpawnChance = newChance;
                    isAdjusted = true;
                }



                EditorGUILayout.EndHorizontal();
            }



            EditorGUILayout.Space();
        }

        // Apply changes
        if (isAdjusted)
        {
            // Mark the target as dirty to save changes
            EditorUtility.SetDirty(target);
            isAdjusted = false;
        }

        base.OnInspectorGUI();
    }

    // Adjust the spawn chances of other enemies to ensure the total chance is 100%
    private void AdjustOtherEnemiesSpawnChance(WaveFormat wave, int changedIndex, int newChance)
    {
        int totalChance = 0;
        foreach (var enemy in wave.possibleEnemiesToSpawn)
        {
            totalChance += enemy.SpawnChance;
        }

        // Calculate the remaining percentage
        int remainingChance = 100 - totalChance + wave.possibleEnemiesToSpawn[changedIndex].SpawnChance;

        // Distribute the remaining chance to other enemies
        for (int i = 0; i < wave.possibleEnemiesToSpawn.Count; i++)
        {
            if (i == changedIndex) continue;  // Skip the changed enemy

            // Calculate how much to adjust based on the remaining chance
            float remainingPercent = (float)wave.possibleEnemiesToSpawn[i].SpawnChance / (totalChance - wave.possibleEnemiesToSpawn[changedIndex].SpawnChance);
            wave.possibleEnemiesToSpawn[i].SpawnChance = Mathf.FloorToInt(remainingChance * remainingPercent);

            // Update the totalChance to reflect the modified value
            totalChance = 0;
            foreach (var enemy in wave.possibleEnemiesToSpawn)
            {
                totalChance += enemy.SpawnChance;
            }
        }
    }
}
[System.Serializable]
public class WaveFormat
{
    [Tooltip("Duration of the wave in seconds")]
    public float durationOfWave;

    [Tooltip("Whether this is a rest wave (no enemies will spawn)")]
    public bool isRestWave;

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
    public Sprite EnemyImage;

    [Tooltip("The enemy's level")]
    public int EnemyLevel;

    [Tooltip("The chance (percentage) that this enemy will spawn in this wave (0-100%)")]
    [Range(0, 100)]
    public int SpawnChance = 0;  // The chance this enemy will spawn (0-100%)

    public string GetEnemyName() => EnemyName = prefab.enemyName;
    public Sprite SetSprite() => EnemyImage = prefab.previewImage;

}
