using UnityEngine;

public class PlayerStats
{
    #region === Variables ===
    // Constants (Defaults)
    private const float DEFAULT_BULLET_SPEED = 5f;
    private const float DEFAULT_RECOIL_SPEED = 3f;
    private const float DEFAULT_MOVEMENT_SPEED = 10f;
    private const int DEFAULT_BULLET_DAMAGE = 5;
    private const int DEFAULT_MAX_HEALTH = 100;
    private const int DEFAULT_LEVEL = 0;
    private const int DEFAULT_MAX_XP = 10;
    #endregion

    #region === Variables (Runtime modifiable) ===
    //Floats
    private float bulletSpeed;
    private float fireRate;
    private float movementSpeed;


    // Ints
    private int bulletDamage;
    private int maxHealth;
    private int currentLevel;
    private int maxXP;

    //Boleans

    #endregion
    // Constructor
    public PlayerStats()
    {
        bulletSpeed = DEFAULT_BULLET_SPEED;
        fireRate = DEFAULT_RECOIL_SPEED;
        movementSpeed = DEFAULT_MOVEMENT_SPEED;

        bulletDamage = DEFAULT_BULLET_DAMAGE;
        maxHealth = DEFAULT_MAX_HEALTH;
        currentLevel = DEFAULT_LEVEL;
        maxXP = DEFAULT_MAX_XP;
    }

    #region === Getters ===
    public float GetBulletSpeed() => bulletSpeed;
    public float GetFireRate() => fireRate;
    public float GetMovementSpeed() => movementSpeed;
    public int GetBulletDamage() => bulletDamage;
    public int GetMaxHealth() => maxHealth;
    public int GetMaxXP() => maxXP;
    public int GetCurrentLevel() => currentLevel;

    #endregion

    #region === Modifiers ===
    public void IncreaseBulletSpeed(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            bulletSpeed += bulletSpeed / increasePercentage;
        }
    }

    public void IncreaseRecoilSpeed(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            if (fireRate > 0.005)
                fireRate -= fireRate / increasePercentage;
        }
    }

    public void IncreaseDamage(float increasePercentage)
    {
        if(increasePercentage > 0)
        {
            bulletDamage += Mathf.Max(1, (int)(bulletDamage / increasePercentage));
        }
    }

    public void IncreaseMaxHealth(float increasePercentage)
    {
        maxHealth += (int)(maxHealth / increasePercentage);
    }

    #endregion
}
