using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPrefab", menuName = "Tools/EnemyPrefab")]
public class EnemyPrefab : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public Sprite previewImage;
}
