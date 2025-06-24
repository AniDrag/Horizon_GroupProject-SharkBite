using UnityEngine;

public class CombatScript : MonoBehaviour,IPooledObject
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletForce = 200;
    private float _fireRate;
    private int _damage;
    private float _lastAttackTime = 0;
    Pooler _pooler;
    private EnemyCore _core;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        _core = GetComponent<EnemyCore>();
        if (_core != null)
        {
            _damage = _core.GetDamage();
            _fireRate = Mathf.Max(_core.GetAttackRatePerSecond(), 0.2f); // Clamp to safe minimum
        }
        _pooler = Pooler.instance;
        if (bulletPrefab == null)
        {
            Debug.LogError("CombatScript: bulletPrefab not assigned.");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _lastAttackTime += Time.deltaTime;
        if (_lastAttackTime >= _fireRate)
        {
            _lastAttackTime = 0;
            Shoot();
        }
    }
    void Shoot()
    {
       
        GameObject newBullet = _pooler.SpawnFromPool("EnemyBullet", transform.position + new Vector3(0,.5f,1.5f), Quaternion.identity);
        newBullet.GetComponent<EnemyDamage>().SetDamage(_damage);


        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //Debug.Log("RB set");
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(transform.forward * bulletForce, ForceMode.Force);
        }
        //Debug.Log("I shot a bullet");
    }

}
