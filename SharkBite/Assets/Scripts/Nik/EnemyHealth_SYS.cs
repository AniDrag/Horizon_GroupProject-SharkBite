using UnityEngine;

public class EnemyHealth_SYS : MonoBehaviour, IPooledObject
{

    [SerializeField] GameObject xpOrbPrefab;
    private int _currentEnemyHealth = 100;
    private int _defese;
    private EnemyCore _enemyCore;
    public int GetEnemyCurrentHealth() => _currentEnemyHealth;

    // list of modifiers and make a class for what specific action it should do.
    private void Start()
    {
        _enemyCore = GetComponent<EnemyCore>();
        _currentEnemyHealth = _enemyCore.GetHealth();
        _defese = _enemyCore.GetDefense();
    }
    public void RespawndObject()
    {
        _currentEnemyHealth = _enemyCore.GetHealth();
        _defese = _enemyCore.GetDefense();
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
        Pooler.instance.SpawnFromPool("XP", transform.position + Vector3.up * 1, Quaternion.identity);
        Spawner.instance.SPAWN_enemysInScene.Remove(gameObject);
        gameObject.SetActive(false);
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
