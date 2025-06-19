using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyHealth_SYS : MonoBehaviour
{
    public int currentEnemyHealth = 100;

    [SerializeField] GameObject xpOrbPrefab;

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
