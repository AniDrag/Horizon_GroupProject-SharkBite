using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test_Editor : MonoBehaviour
{
    public enum EnemyTypes
    {
        Mele,
        Ranged,
        Mix
    }
    [SerializeField] EnemyTypes enemyType;
    [SerializeField] List<GameObject> testList = new List<GameObject>();
    [SerializeField] float meleDamage;
    [SerializeField] float magicDamage;

    [SerializeField] float meleSpeed;
    [SerializeField] float magicSpeed;

    [SerializeField] int defense;
    [SerializeField] int health;
    [SerializeField] int level;

    [SerializeField] Image previewImage;

    
}
