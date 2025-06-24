using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    
    private int _damage = 5;
    bool _triggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDamage(int setDamageTo)
    {
        _damage = setDamageTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth_SYS playerHealth = other.gameObject.GetComponent<PlayerHealth_SYS>();
        
        if (playerHealth != null && !_triggered)
        {
            _triggered = true;
            playerHealth.TakeDamage(_damage);
            Debug.Log($"Damage delt {_damage} to target");
        }

        _triggered = false;
        this.gameObject.SetActive(false);
    }
}
