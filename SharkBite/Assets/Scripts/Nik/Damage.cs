using UnityEngine;

public class Damage : MonoBehaviour
{
    private int _damage = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDamage(int setDamageTo)
    {
        _damage = setDamageTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth_SYS playerHealth = other.gameObject.GetComponent<PlayerHealth_SYS>();
        EnemyHealth_SYS enemyhealth = other.gameObject.GetComponent<EnemyHealth_SYS>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(_damage);
            Debug.Log($"Damage delt {_damage} to target");
        }
        else if (enemyhealth !=null)
        {
            enemyhealth.TakeDamage(_damage);
            Debug.Log($"Damage delt {_damage} to target");
        }

        Destroy(this.gameObject);
    }
}
