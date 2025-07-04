using UnityEngine;

public class BossSpecificWeapon : MonoBehaviour
{
    private float _fireRate = 1;// Attack rate
    private int _damage = 100;
    private float _lastAttackTime;
    BossCore _core;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _core = transform.parent.GetComponent<BossCore>();
        if (_core != null)
        {
            _damage = _core.GetDamage();
            _fireRate = _core.GetAttackRatePerSecond();
        }
        else
        {
            Debug.LogError("Null refrence to boss Core!!");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Boss trigger 1");
        if (other.CompareTag("Player") && Time.time >= _lastAttackTime + _fireRate)
        {
            Debug.Log("Boss recognised player 2");
            Attack(other.gameObject);
        }
    }

    void Attack(GameObject other)
    {

        Debug.Log("Playr attacked by Boss");
        _lastAttackTime = Time.time;
        other.GetComponent<PlayerHealth_SYS>().TakeDamage(_damage);

    }
}
