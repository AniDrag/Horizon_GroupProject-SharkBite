using UnityEngine;
[RequireComponent(typeof(EnemyHealth_SYS))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyCore : MonoBehaviour
{
    public enum EnemyType
    {
        None,
        Ranged,
        CloseRange
    }
    [SerializeField] public EnemyType enemyType;
    // Header here with all the basic stuff
    [SerializeField] private int maxHealth;
    [SerializeField] private int defense;
    [SerializeField] private int damage;
    [SerializeField] private float attackRatePerSecond;
    [SerializeField] private float movemantSpeed;

    public GameObject weaponPrefab;


    public int GetHealth() => maxHealth;
    public int GetDefense() => defense;
    public int GetDamage() => damage;
    public float GetAttackRatePerSecond() => attackRatePerSecond;
    public float GetMovementSpeed() => movemantSpeed;
    public bool GetEnemyType() => enemyType == EnemyType.Ranged;

    public int SetHealth(int newValue) => maxHealth = newValue;
    public int SetDefense(int newValue) => defense = newValue;
    public int SetDamage(int newValue) => damage = newValue;
    public float SetAttackRatePerSecond(float newValue) => attackRatePerSecond = newValue;
    public float SetMovementSpeed (float newValue) => movemantSpeed = newValue;


    //repeat for all accept enum
}
