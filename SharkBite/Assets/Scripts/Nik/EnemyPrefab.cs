using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EnemyPrefab", menuName = "Tools/EnemyPrefab")]
public class EnemyPrefab : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public Image previewImage;
}
