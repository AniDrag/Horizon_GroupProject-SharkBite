using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("===== Weapon Refrences =====")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform orientation;

    private PlayerStats playerStats;
    private float _timeReset;
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
        _timeReset += Time.deltaTime;

        
    }
    private void FixedUpdate()
    {
        if (_timeReset >= playerStats.GetFireRate())
        {
            // Debug.Log("I shot a bullet");
            Shoot(playerStats.GetFireRate(), orientation.position);
            _timeReset = 0;
        }
    }

    private void Shoot(float cd, Vector3 moveDirection)
    {

        GameObject newBullet = Instantiate(projectilePrefab, orientation.position + orientation.forward * 1.5f, Quaternion.identity);
        newBullet.GetComponent<Damage>().SetDamage(playerStats.GetBulletDamage());

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(orientation.forward * playerStats.GetBulletSpeed() * 200, ForceMode.Force);
        }
        Debug.Log("I shot a bullet");
    }
}
