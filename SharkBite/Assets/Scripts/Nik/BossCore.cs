using System.Collections.Generic;
using UnityEngine;

public class BossCore : EnemyCore
{
    public enum BossPhase { Idle, ChargeAttack, Summon }
    private BossPhase currentPhase = BossPhase.Idle;

    [SerializeField]private Animator animator;
    private Transform player;
    private float timer;
    private float baseAttackSpeed;
    EnemyHealth_SYS enemyhealth;
    int health;
    int health_75;
    int health_50;
    int health_25;
    bool attacking;

    [Header("Phase Timings")]
    [SerializeField][Range(1, 20)] int spawnAliesCout;
    [SerializeField] private List<EnemyPrefab> spawnableEnemies;
    [SerializeField] private List<Animation> allBossAnims;
    private void Start()
    {
        player = GameManager.instance.Player;
        enemyhealth = GetComponent<EnemyHealth_SYS>();
        health = enemyhealth.GetEnemyCurrentHealth();
        health_75 -= health /4;
        health_50 -= health /2;
        health_25 = health / 4;
        baseAttackSpeed = GetAttackRatePerSecond();
    }
    void Update()
    {
        timer += Time.deltaTime;
        health = enemyhealth.GetEnemyCurrentHealth();
        if (health == health_75 || health == health_50 || health == health_25) Enrage();
        else if (health <= 0)
        {
            animator.SetBool("Death", true);
        }

        if (timer > baseAttackSpeed && !attacking) return;
        #region My lazy as calculation
        float totalWeight = 10 + 3;
        float rand = Random.Range(0f, totalWeight);
        if (rand < 10)
            currentPhase =BossPhase.ChargeAttack;
        else
            currentPhase = BossPhase.Summon;
        timer = 0;
        #endregion

        switch (currentPhase)
        {
            case BossPhase.ChargeAttack:
                Charge();                
                break;
            case BossPhase.Summon:
                Summon();
                break;
        }
    } 

    void Charge()
    {
        attacking = true;

        animator.SetBool("Charging", true);
        #region Another lazy ass calculation
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 targetPos = transform.position + direction * (distance + 5f);
        float stopDistance = Vector3.Distance(transform.position, targetPos);
        #endregion
        // Optional: animation trigger

        float duration = 0.5f;
        float t = 0f;
        Vector3 start = transform.position;

        while (3 < stopDistance)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPos, t / duration);
            //yield return null;
        }
        animator?.SetBool("Charging", false);
        timer = 0;
        attacking = false;
    }

    void Summon()
    {
        attacking = true;
        currentPhase = BossPhase.Summon;
        for (int i = 0; i < spawnAliesCout; i++)
        {
            EnemyPrefab enemyToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count - 1)];
            Vector3 playerPos = player.position;

            float angle = Random.Range(0, Mathf.PI * 2);

            Vector3 rndPos = new Vector3(
                playerPos.x + 10 * Mathf.Cos(angle),
                0,
                playerPos.z + 10 * Mathf.Sin(angle)
            );

            Pooler.instance.SpawnFromPool(enemyToSpawn.enemyName, rndPos, Quaternion.identity);
        }
        Invoke(nameof(ResetSummon), 2);
    }
    void ResetSummon()
    {
        timer = 0;
        attacking = false;
    }
    public void Enrage()
    {
        baseAttackSpeed *= 0.5f;
        
        SetMovementSpeed( GetMovementSpeed()*1.2f);
    }

}
