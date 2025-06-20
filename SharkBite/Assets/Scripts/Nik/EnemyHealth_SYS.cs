using UnityEngine;

public class EnemyHealth_SYS : MonoBehaviour
{

    [SerializeField] GameObject xpOrbPrefab;
    private int currentEnemyHealth = 100;
    private void Start()
    {
        currentEnemyHealth = GetComponent<EnemyMovement>().GetHealth(); // ALWAYS RETURNS 1, HAS TO BE MADE
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            return;

        int tempHelth = currentEnemyHealth - DamageCalculationWithModifiers(damage);

        if (tempHelth <= 0)
        {
            currentEnemyHealth = 0;
            gameObject.SetActive(false);

        }
        else
        {
            currentEnemyHealth = tempHelth;
        }


    }

    void IsEnemyLogic()
    {
        Instantiate(xpOrbPrefab, transform.position + Vector3.up * 1, Quaternion.identity);
        Spawner.instance.SPAWN_enemysInScene.Remove(gameObject);
        Destroy(gameObject);
    }

    int DamageCalculationWithModifiers(int damage)
    {
        return damage;
    }
}
