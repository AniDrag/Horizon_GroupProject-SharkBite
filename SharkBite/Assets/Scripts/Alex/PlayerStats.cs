using UnityEngine;
using System;
public class PlayerStats
{
    #region === Variables ===
    // Constants (Defaults)
    private const int DEFAULT_MAX_HEALTH = 100;
    private const int DEFAULT_DEFENSE = 3;
    private const int DEFAULT_LEVEL = 0;
    private const int DEFAULT_MAX_XP = 5;
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

    #region === Helper Functions ===
    /// <summary>
    /// Increase a float stat by a percentage of itself, then fire OnStatsChanged if >0.
    /// </summary>
    private void IncreaseFloat(ref float stat, float percent)
    {
        if (percent <= 0f) return;
        stat += stat * (percent / 100f);
        OnStatsChanged?.Invoke();
    }

    /// <summary>
    /// Decrease a float stat by a percentage of itself, with an optional minimum clamp.
    /// </summary>
    private void DecreaseFloat(ref float stat, float percent, float minValue = 0f)
    {
        if (percent <= 0f) return;
        stat -= stat * (percent / 100f);
        stat = Mathf.Max(stat, minValue);
        OnStatsChanged?.Invoke();
    }

    /// <summary>
    /// Increase an integer stat by a percentage of itself, rounding at least +1.
    /// </summary>
    private void IncreaseInt(ref int stat, float percent)
    {
        if (percent <= 0f) return;
        int delta = Mathf.Max(1, Mathf.FloorToInt(stat * (percent / 100f)));
        stat += delta;
        OnStatsChanged?.Invoke();
    }

#endregion

    #region === Modifiers ===

    // === Public API ===

    public void IncreaseBulletSpeed(float increasePercentage)
    {
        IncreaseFloat(ref bulletSpeed, increasePercentage);
    }

    public void IncreaseRecoilSpeed(float increasePercentage)
    {
        DecreaseFloat(ref fireRate, increasePercentage, minValue: 0.005f);
    }

    public void IncreaseDamage(float increasePercentage)
    {
        IncreaseInt(ref bulletDamage, increasePercentage);
    }

    public void IncreaseMaxHealth(float increasePercentage)
    {
        IncreaseInt(ref maxHealth, increasePercentage);
    }

    public void IncreaseDefense(float increasePercentage)
    {
        IncreaseInt(ref defense, increasePercentage);
    }
    #endregion
}
