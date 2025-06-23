using UnityEngine;

public class EnemyHealth_SYS : MonoBehaviour
{

    [SerializeField] GameObject xpOrbPrefab;
    private int _currentEnemyHealth = 100;
    private int _defese;

    public int GetEnemyHealth() => _currentEnemyHealth;

    // list of modifiers and make a class for what specific action it should do.
    private void Start()
    {
        _currentEnemyHealth = GetComponent<EnemyCore>().GetHealth();
        _defese = GetComponent<EnemyCore>().GetDefense();
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) return;

        int tempHelth = _currentEnemyHealth - DamageCalculationWithModifiers(damage);

        if (tempHelth <= 0)
        {
            _currentEnemyHealth = 0;
            OnDeath();
        }
        else
        {
            _currentEnemyHealth = tempHelth;
        }
    }

    void OnDeath()
    {
        Instantiate(xpOrbPrefab, transform.position + Vector3.up * 1, Quaternion.identity);
        Spawner.instance.SPAWN_enemysInScene.Remove(gameObject);
        Destroy(this.gameObject);
    }

    int DamageCalculationWithModifiers(int damage)
    {
        damage -= _defese;
        if (damage < 0)
        {
            damage = 0;
        }
        return damage;
    }
}
