using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private float _fireRate;
    private int _damage;
    private float _lastAttackTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private EnemyCore _data;
    void Start()
    {
        _data = transform.parent.GetComponent<EnemyCore>();
        _damage = _data.GetDamage();
        _fireRate = _data.GetAttackRatePerSecond();
        Collider col = gameObject.GetComponent<Collider>();
        col.isTrigger = true;
       
    }

    private void OnTriggerStay(Collider other)
    { 
        if(other.CompareTag("Player"))
        {
            Attack(other.gameObject);            
        }

    }

    void Attack(GameObject other ) {

        if (Time.time >= _lastAttackTime + _fireRate)
        {
            _lastAttackTime = Time.time;
            other.GetComponent<PlayerHealth_SYS>().TakeDamage(_damage);
        }
    }

}
