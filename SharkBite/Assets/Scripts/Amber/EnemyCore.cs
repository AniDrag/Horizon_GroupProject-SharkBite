using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    public enum EnemyType
    {
        Ranged,
        CloseRange
    }
    [SerializeField] public EnemyType enemyType;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damageOutput;
    [SerializeField] private float attackRatePerSecond;
    [SerializeField] private float movemantSpeed;

    public int GetHealth() => currentHealth;
    public int GetDamage() => damageOutput;
    public float GetAttackRatePerSecond() => attackRatePerSecond;
    public float GetMovementSpeed() => movemantSpeed;
    public bool GetEnemyType() => enemyType == EnemyType.Ranged;
  
    //repeat for all accept enum
}
