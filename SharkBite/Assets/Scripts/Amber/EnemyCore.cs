using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(EnemyHealth_SYS))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyCore : MonoBehaviour,IPooledObject
{
    public enum EnemyType
    {
        None,
        Ranged,
        CloseRange,
        Rusher
    }
    [SerializeField] public EnemyType enemyType;
    // Header here with all the basic stuff
    [SerializeField] private int maxHealth;
    [SerializeField] private int defense;
    [SerializeField] private int damage;
    [SerializeField] private float attackRatePerSecond;
    [SerializeField] private float movemantSpeed;
    [SerializeField] private Image enemyImage;
    [SerializeField] Transform orientation;

    public GameObject weaponPrefab;
    private void Start()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        orientation = transform.GetChild(0);
        enemyImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    public int GetHealth() => maxHealth;
    public int GetDefense() => defense;
    public int GetDamage() => damage;
    public float GetAttackRatePerSecond() => attackRatePerSecond;
    public float GetMovementSpeed() => movemantSpeed;
    public bool GetEnemyType() => enemyType == EnemyType.Ranged;
    public Transform GetOrientation() => orientation;

    public int SetHealth(int newValue) => maxHealth = newValue;
    public int SetDefense(int newValue) => defense = newValue;
    public int SetDamage(int newValue) => damage = newValue;
    public float SetAttackRatePerSecond(float newValue) => attackRatePerSecond = newValue;
    public float SetMovementSpeed (float newValue) => movemantSpeed = newValue;

    public void SetImage(Sprite newImage) => enemyImage.sprite = newImage;
    public void SetImageScale(float scale = 1) => enemyImage.transform.localScale = Vector3.one * scale;


    //repeat for all accept enum
}
