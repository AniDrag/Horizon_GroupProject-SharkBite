using UnityEngine;

public class CombatScript : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    Transform _firePoint;

    private float _fireRate;
    private int _damage;
    private float _lastAttackTime = 0;
    private bool _isRanged;
    Pooler _pooler;
    private EnemyCore _core;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _core = GetComponent<EnemyCore>();
        if (_core != null)
        {
            _damage = _core.GetDamage();
            _fireRate = Mathf.Max(_core.GetAttackRatePerSecond(), 0.2f); // Clamp to safe minimum
            _isRanged = _core.GetEnemyType(); // if this method exists
        }
        _pooler = Pooler.instance;
        if (bulletPrefab == null)
        {
            Debug.LogError("CombatScript: bulletPrefab not assigned.");
            enabled = false;
        }

        if (_firePoint == null)
        {
            // Default fallback if you forgot to assign it
            GameObject firePointGO = new GameObject("FirePoint");
            firePointGO.transform.parent = transform;
            firePointGO.transform.localPosition = new Vector3(0f, 0.5f, 1f);
            _firePoint = firePointGO.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRanged) return;

        if (Time.time >= _lastAttackTime + _fireRate)
        {
            _lastAttackTime = Time.time;
            Shoot();
        }
    }
    void Shoot()
    {
        // Option 1: If using pooling
        GameObject bullet = _pooler.SpawnFromPool("Bullet", _firePoint.position, Quaternion.identity);

        // Option 2: If NOT using pooling (fallback)
        // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Damage dmg = bullet.GetComponent<Damage>();
        if (dmg == null) {
            Debug.LogWarning("No damage script on bullet");
            return;
        }
        dmg.SetDamage(_damage);
        

        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero; // Reset before applying new force
            rb.AddForce(_firePoint.forward * 200f, ForceMode.Force);
        }

        // Avoid spamming logs
        // Debug.Log($"Spawned bullet with damage: {_damage}");
    }

}
