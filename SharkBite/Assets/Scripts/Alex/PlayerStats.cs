using UnityEngine;

public class PlayerStats
{

    // Constants (Defaults)
    private const float DEFAULT_BULLET_SPEED = 5f;
    private const float DEFAULT_RECOIL_SPEED = 3f;
    private const float DEFAULT_MOVEMENT_SPEED = 10f;
    private const int DEFAULT_BULLET_DAMAGE = 5;
    private const int DEFAULT_MAX_HEALTH = 100;

    // Variables (Runtime modifiable)
    private float bulletSpeed;
    private float recoilSpeed;
    private float movementSpeed;

    private int bulletDamage;
    private int maxHealth;

    // Constructor
    public PlayerStats()
    {
        bulletSpeed = DEFAULT_BULLET_SPEED;
        recoilSpeed = DEFAULT_RECOIL_SPEED;
        movementSpeed = DEFAULT_MOVEMENT_SPEED;

        bulletDamage = DEFAULT_BULLET_DAMAGE;
        maxHealth = DEFAULT_MAX_HEALTH;
    }

    // === Getters ===
    public float GetBulletSpeed() => bulletSpeed;
    public float GetRecoilSpeed() => recoilSpeed;
    public float GetMovementSpeed() => movementSpeed;
    public int GetBulletDamage() => bulletDamage;
    public int GetMaxHealth() => maxHealth;


    // === Modifiers ===
    public void IncreaseBulletSpeed(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            bulletSpeed = bulletSpeed + (bulletSpeed / increasePercentage);
        }
    }

    public void IncreaseRecoilSpeed(float increasePercentage)
    {
        if (increasePercentage > 0)
        {
            if (recoilSpeed > 0.005)
                recoilSpeed = recoilSpeed - (recoilSpeed / increasePercentage);
        }
    }

    public void IncreaseDamage(float increasePercentage)
    {
        if(increasePercentage > 0)
        {
            bulletDamage = bulletDamage + Mathf.Max(1, (int)(bulletDamage / increasePercentage));
        }
    }
}
