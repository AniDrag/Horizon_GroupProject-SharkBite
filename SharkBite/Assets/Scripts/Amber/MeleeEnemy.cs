using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private float _fireRate = 1;
    private int _damage = 10;
    private float _lastAttackTime;
    public bool isCrab;
    Manager_Sound _audio;
    public Animator _animation;
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
        _animation = _data.GetAnimator();

    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("trigger 1");
        if (other.CompareTag("Player"))
        {
            //Debug.Log("triggger 2");
            Attack(other.gameObject);
        }

    }

    void Attack(GameObject other ) {

        if (Time.time >= _lastAttackTime + _fireRate)
        {           
            _animation.SetTrigger("isAttacking");
           // Debug.Log("Playr attacked by mele");
            _lastAttackTime = Time.time;
            other.GetComponent<PlayerHealth_SYS>().TakeDamage(_damage);
            _audio.PlaySFX(isCrab? _audio.crabSound : _audio.piranahSound);
        }
    }

}
