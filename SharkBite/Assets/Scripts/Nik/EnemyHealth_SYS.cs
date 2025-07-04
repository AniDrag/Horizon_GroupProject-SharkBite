using UnityEngine;

public class EnemyHealth_SYS : DamageShader, IPooledObject
{

    private int _currentEnemyHealth = 100;
    private int _defese;
    private EnemyCore _enemyCore;
    Manager_Sound _audio;
    private bool bossSpawn;
    [SerializeField] bool isBoss;
    public int GetEnemyCurrentHealth() => _currentEnemyHealth;
    public void IsBossSpawn() => bossSpawn = true;

    // list of modifiers and make a class for what specific action it should do.
    private void Start()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        _enemyCore = GetComponent<EnemyCore>();
        _currentEnemyHealth = _enemyCore.GetHealth();
        _defese = _enemyCore.GetDefense();
        _audio = Manager_Sound.instance;
    }
    public void TakeDamage(int damage)
    {
        if (damage < 0) return;
        Debug.Log($"I took damage Enemy-{damage}");
        int tempHelth = _currentEnemyHealth - DamageCalculationWithModifiers(damage);
        Debug.Log($"Actual damage I took --> Enemy-{DamageCalculationWithModifiers(damage)}");
        _audio?.PlaySFX(_audio.enemyDamaged);
        if (tempHelth <= 0)
        {
            _currentEnemyHealth = 0;
            OnDeath();
        }
        else
        {
            _currentEnemyHealth = tempHelth;
            StartCoroutine(DamageAnimation());
        }
    }

    void OnDeath()
    {
        Pooler.instance.SpawnFromPool("XP", transform.position + Vector3.up * 1, Quaternion.identity);
        _audio.PlaySFX(_audio.enemyDethSound);

        if (Spawner_var2.instance._enemiesOnScreen.Contains(gameObject))
        {
            Spawner_var2.instance._enemiesOnScreen.Remove(gameObject);
        } // Delete game object

        if (isBoss)
        {
            return;
        }

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
