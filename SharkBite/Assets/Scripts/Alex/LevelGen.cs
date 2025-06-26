using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour
{
    [Header("========= Obstacle settings =========")]

    [Tooltip("Min and Max coordinates from my position to spawn obstacle")]
    [SerializeField][MinMaxSlider(-200, 200)] private Vector2Int xBoundaries = new Vector2Int();
    [Tooltip("Min and Max coordinates from my position to spawn obstacle")]
    [SerializeField][MinMaxSlider(-200, 200)] private Vector2Int zBoundaries = new Vector2Int();

    [Tooltip("List of obstacles for the level")]
    [SerializeField] private List<ObstacleDetails> obstacleList = new List<ObstacleDetails>();
    // Tracks used grid positions to prevent overlap
    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
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
        int totalChance = 0;
        foreach (var detail in obstacleList)
        {
            totalChance += detail.spawnChance;
        }

        if (totalChance == 0)
        {
            Debug.LogWarning("LevelGenerator: Total spawn chance is 0.");
            return;
        }

        int spawnCount = Mathf.Min(50, totalChance); // Cap to avoid excessive spawns
        for (int i = 0; i < spawnCount; i++)
        {
            int rand = Random.Range(0, totalChance);
            int currentSum = 0;
            foreach (var detail in obstacleList)
            {
                currentSum += detail.spawnChance;
                if (rand < currentSum)
                {
                    int count = Random.Range(detail.amountChance.x, detail.amountChance.y + 1);
                    for (int j = 0; j < count; j++)
                    {
                        bool isGroup = (Random.Range(0, 100) < detail.groupChance);
                        if (isGroup)
                        {
                            int groupSize = Random.Range(2, Mathf.Max(3, (int)detail.volume.magnitude));
                            int density = Mathf.CeilToInt(Mathf.Sqrt(groupSize));
                            SpawnMultiple(detail, groupSize, density);
                        }
                        else
                        {
                            Vector2Int gridPos = GetRandomGridPosition();
                            if (CanOccupy(gridPos, detail.volume))
                            {
                                SpawnSingle(detail, gridPos);
                            }
                        }
                    }
                    break;
                }
            }
        }
    }

    private void SpawnMultiple(ObstacleDetails detail, int size, int density)
    {
        // Use SpawnSingle to spawn every object + offset, that depends on the size and density
        Vector2Int center = GetRandomGridPosition();
        for (int i = 0; i < size; i++)
        {
            int row = i / density;
            int col = i % density;
            float offsetX = (col - (density - 1) * 0.5f) * detail.volume.x;
            float offsetZ = (row - (density - 1) * 0.5f) * detail.volume.y;
            Vector2Int spawnGrid = new Vector2Int(center.x + Mathf.RoundToInt(offsetX), center.y + Mathf.RoundToInt(offsetZ));
            if (CanOccupy(spawnGrid, detail.volume))
            {
                SpawnSingle(detail, spawnGrid);
            }
        }
    }

    private void SpawnSingle(ObstacleDetails detail, Vector2Int gridPosition)
    {
        float scale = Random.Range(detail.scale.x, detail.scale.y);
        float yOffset = detail.yToScale * scale;
        Vector3 worldPos = new Vector3(gridPosition.x, yOffset, gridPosition.y);

        GameObject instance = Instantiate(detail.prefab, worldPos, Quaternion.identity, transform);
        instance.transform.localScale = Vector3.one * scale;

        MarkOccupied(gridPosition, detail.volume);
    }

    private Vector2Int GetRandomGridPosition()
    {
        const int maxAttempts = 100;
        Vector2Int candidate;
        int attempts = 0;
        do
        {
            int x = Random.Range(xBoundaries.x, xBoundaries.y + 1);
            int z = Random.Range(zBoundaries.x, zBoundaries.y + 1);
            candidate = new Vector2Int(x, z);
            attempts++;
        }
        while (occupiedPositions.Contains(candidate) && attempts < maxAttempts);

        return candidate;
    }

    private bool CanOccupy(Vector2Int origin, Vector2 volume)
    {
        for (int x = 0; x < Mathf.CeilToInt(volume.x); x++)
        {
            for (int y = 0; y < Mathf.CeilToInt(volume.y); y++)
            {
                Vector2Int pos = new Vector2Int(origin.x + x, origin.y + y);
                if (occupiedPositions.Contains(pos))
                    return false;
            }
        }
        return true;
    }

    private void MarkOccupied(Vector2Int origin, Vector2 volume)
    {
        for (int x = 0; x < Mathf.CeilToInt(volume.x); x++)
        {
            for (int y = 0; y < Mathf.CeilToInt(volume.y); y++)
            {
                occupiedPositions.Add(new Vector2Int(origin.x + x, origin.y + y));
            }
        }
    }
}
