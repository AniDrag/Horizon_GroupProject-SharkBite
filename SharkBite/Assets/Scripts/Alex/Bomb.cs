using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 5;
    [SerializeField] private int damageToEnemy = 5;
    [SerializeField] private float bombRadius;
    [SerializeField] private LayerMask mask;
    [SerializeField] private bool showRadius;

    bool _triggered;

    private void OnDrawGizmos()
    {
        if (showRadius)
            Gizmos.DrawSphere(transform.position, bombRadius);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.CompareTag("Bullet"))
        {
            _triggered = true;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, bombRadius, mask);
            foreach (Collider col in hitColliders)
            {
                PlayerHealth_SYS playerHealth = col.gameObject.GetComponent<PlayerHealth_SYS>();
                EnemyHealth_SYS enemyhealth = col.gameObject.GetComponent<EnemyHealth_SYS>();
                if (enemyhealth != null)
                {
                    enemyhealth.TakeDamage(damageToEnemy);
                    Debug.Log($"Damage delt {damageToEnemy} to target");
                }
                else if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);
                    Debug.Log($"Damage delt {damageToPlayer} to target");
                }
            }
            Destroy(this.gameObject);
        }
    }
}
