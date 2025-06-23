using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour
{
    [Header("========= Obstacle settings =========")]

    [Tooltip("Min and Max coordinates from my position to spawn obstacle")]
    [SerializeField][MinMaxSlider(-100, 100)] private Vector2Int xBoundaries = new Vector2Int();
    [Tooltip("Min and Max coordinates from my position to spawn obstacle")]
    [SerializeField][MinMaxSlider(-100, 100)] private Vector2Int zBoundaries = new Vector2Int();

    [Tooltip("List of obstacles for the level")]
    [SerializeField] private List<ObstacleDetails> obstacleList = new List<ObstacleDetails>();
    void Start()
    {
        if (obstacleList.Count != 0)
        {
            GenerateLevel();
        }
        else
            Debug.LogWarning("LevelGen is missing obstacles to work with");
    }

    private void GenerateLevel()
    {
        // Select a random obstacle, maybe with different chances, depending on a seed
        // Check the chance of it to be many of them, determine size by the chance as well
        // Select a position, based on whether this position is already used
        // TODO: Ask Nik to check if this position is used, so the spawner doesn't spawn things inside obstacles?

        
    }

    private void SpawnMultiple(int index, int size, int density)
    {
        // Use SpawnSingle to spawn every object + offset, that depends on the size and density
        
    }

    private void SpawnSingle(int index, Vector2 position)
    {
        // Spawn object, mark the position as used.
    }
}
