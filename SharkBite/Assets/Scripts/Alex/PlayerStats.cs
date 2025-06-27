using UnityEngine;
using System;
public class PlayerStats
{
    #region === Variables ===
    // Constants (Defaults)
    private const int DEFAULT_MAX_HEALTH = 100;
    private const int DEFAULT_DEFENSE = 3;
    private const int DEFAULT_LEVEL = 0;
    private const int DEFAULT_MAX_XP = 10;
    private const int DEFAULT_BULLET_DAMAGE = 50;
    private const float DEFAULT_BULLET_SPEED = 5f;
    private const float DEFAULT_FIRE_RATE = 0.5f;
    private const float DEFAULT_MOVEMENT_SPEED = 6f;
    #endregion

    #region === Variables (Runtime modifiable) ===
    // Ints
    private int maxHealth;
    private int defense;
    private int currentLevel;
    private int maxXP;
    private int bulletDamage;

    //Floats
    private float bulletSpeed;
    private float fireRate;
    private float movementSpeed;
    

    #endregion
    // Constructor
    public PlayerStats()
    {
        maxHealth = DEFAULT_MAX_HEALTH;
        defense = DEFAULT_DEFENSE;
        currentLevel = DEFAULT_LEVEL;
        maxXP = DEFAULT_MAX_XP;


        bulletDamage = DEFAULT_BULLET_DAMAGE;
        bulletSpeed = DEFAULT_BULLET_SPEED;
        fireRate = DEFAULT_FIRE_RATE;
        movementSpeed = DEFAULT_MOVEMENT_SPEED;
    }
    #region === Events ===
    public event Action OnStatsChanged; // Event to notify when any stat changes
    #endregion
    #region === Getters ===
    public int GetMaxHealth() => maxHealth;
    public int GetDefense() => defense;
    public int GetMaxXP() => maxXP;
    public int GetCurrentLevel() => currentLevel;
    public int GetBulletDamage() => bulletDamage;
    public float GetBulletSpeed() => bulletSpeed;
    public float GetFireRate() => fireRate;
    public float GetMovementSpeed() => movementSpeed;

    #endregion

    #region === Modifiers ===
    public void IncreaseBulletSpeed(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            bulletSpeed += bulletSpeed / increasePercentage;
            OnStatsChanged?.Invoke();  // Trigger the event when the stat is modified
        }
    }

    public void IncreaseRecoilSpeed(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            if (fireRate > 0.005)
                fireRate -= fireRate / increasePercentage;
            OnStatsChanged?.Invoke();  // Trigger the event when the stat is modified
        }
    }

    public void IncreaseDamage(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            bulletDamage += Mathf.Max(1, (int)(bulletDamage / increasePercentage));
            OnStatsChanged?.Invoke();  // Trigger the event when the stat is modified
        }
    }

    public void IncreaseMaxHealth(float increasePercentage)
    {
        maxHealth += (int)(maxHealth / increasePercentage);
        OnStatsChanged?.Invoke();  // Trigger the event when the stat is modified
    }

    public void IncreaseDefense(float increasePercentage)
    {
        defense += (int)(defense / increasePercentage);
        OnStatsChanged?.Invoke();  // Trigger the event when the stat is modified
    }
    #endregion
}
