using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private float _fireRate;
    private int _damage;
    private float _lastAttackTime;
    public bool isCrab;
    Manager_Sound _audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private EnemyCore _data;
    void Start()
    {
        _data = transform.parent.parent.GetComponent<EnemyCore>();
        _damage = _data.GetDamage();
        _fireRate = _data.GetAttackRatePerSecond();
        Collider col = gameObject.GetComponent<Collider>();
        col.isTrigger = true;
        _audio = Manager_Sound.instance;
       
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
            Debug.Log("Playr attacked by mele");
            _lastAttackTime = Time.time;
            other.GetComponent<PlayerHealth_SYS>().TakeDamage(_damage);
            _audio.PlaySFX(isCrab? _audio.crabSound : _audio.piranahSound);
        }
    }

}
