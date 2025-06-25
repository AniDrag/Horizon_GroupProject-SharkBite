using UnityEngine;

public class CombatScript : MonoBehaviour,IPooledObject
{
    [SerializeField] Transform orientation;
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
        if (_core == null)
        {
            Debug.LogError("no enemy core founf");
        }

        _damage = _core.GetDamage();
        _fireRate = Mathf.Max(_core.GetAttackRatePerSecond(), 0.2f); // Clamp to safe minimum

        _pooler = Pooler.instance;
        if (_pooler == null)
        {
            Debug.LogError("no Pooler founf");
        }
        orientation = _core.GetMyOrientation();
        if (orientation == null)
        {
            Debug.LogError("no Orientation founf");
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
       
        GameObject newBullet = _pooler.SpawnFromPool("EnemyBullet", orientation.position + new Vector3(0,.5f,1.5f), Quaternion.identity);
        newBullet.GetComponent<EnemyDamage>().SetDamage(_damage);


        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //Debug.Log("RB set");
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(orientation.forward * bulletForce, ForceMode.Force);
        }
        //Debug.Log("I shot a bullet");
    }

}
