using UnityEngine;

public class CombatScript : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    private float _fireRate;
    private int _damage;
    private float _lastAttackTime = 0;
    private bool _isRanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _damage = GetComponent<EnemyCore>().GetDamage();
        _fireRate = GetComponent<EnemyCore>().GetAttackRatePerSecond();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRanged)
        {
            if (Time.time >= _lastAttackTime + _fireRate)
            {
                _lastAttackTime = Time.time;
                Shoot();
            }
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 1), transform.rotation);
        bullet.GetComponent<Damage>().SetDamage(_damage);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.Force);

        Debug.Log($"Spawned bullet with damage: {_damage}");
    }

}
