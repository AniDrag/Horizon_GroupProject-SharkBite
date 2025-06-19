using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("===== Weapon Refrences =====")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform orientation;

    private PlayerStats playerStats;
    private float _lastShotTime;
    void Start()
    {
        playerStats = PlayerManager.instance.playerStats;
        if (projectilePrefab.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Bullet doesn't have RigidBody");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= _lastShotTime + playerStats.GetFireRate())
        {
            Shoot(playerStats.GetFireRate(), orientation.position);
            _lastShotTime = Time.time;
        }
    }

    private void Shoot(float cd, Vector3 moveDirection)
    {

        GameObject newBullet = Instantiate(projectilePrefab, transform.position + moveDirection, Quaternion.identity);
        newBullet.GetComponent<Damage>().SetDamage(playerStats.GetBulletDamage());

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(moveDirection.normalized * playerStats.GetBulletSpeed());
        }
    }
}
