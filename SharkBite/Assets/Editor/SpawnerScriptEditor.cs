using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner_var2))]
public class SpawnerScriptEditor : Editor
{

    private bool isAdjusted = false; // Flag to check if adjustments have been made

    public override void OnInspectorGUI()
    {
        Spawner_var2 spawner = (Spawner_var2)target;

        // Loop through each wave in the spawner
        foreach (var wave in spawner.waveList)
        {
            int totalChance = 0;
            EditorGUILayout.LabelField("Wave Duration: " + wave.durationOfWave);
            EditorGUILayout.LabelField("Is Rest Wave: " + wave.isRestWave.ToString());
            EditorGUILayout.LabelField("ChanceSum: " + wave.chanceSum);
            // Ensure the total chance doesn't exceed 100%
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
                wave.chanceSum = wave.possibleEnemiesToSpawn[i].SpawnChance;
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