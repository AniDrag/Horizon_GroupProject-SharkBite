using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : EnemyCore
{
    public enum BossPhase { Idle, ChargeAttack, Summon }
    private BossPhase currentPhase = BossPhase.Idle;

    [Header("Attack Settings")]
    [SerializeField] private int chargeWeight = 10;
    [SerializeField] private int summonWeight = 3;

    [SerializeField]private Animator animator;
    private Transform player;
    private float baseAttackSpeed;
    EnemyHealth_SYS enemyhealth;
    int health;
    int health_75;
    int health_50;
    int health_25;

    [Header("Phase Timings")]
    [SerializeField][Range(1, 20)] int spawnAliesCout;
    [SerializeField] private List<EnemyPrefab> spawnableEnemies;
    private RectTransform hpSlider;

    [Header("Rotation")]
    [SerializeField] private Transform rotateMeshHolder;

    private Coroutine attackRoutine;
    private bool isDead;
    float _maxHealth;
    private void Awake()
    {


        player = GameManager.instance.Player;
        enemyhealth = GetComponent<EnemyHealth_SYS>();
        health = enemyhealth.GetEnemyCurrentHealth();
        _maxHealth = GetHealth();
        health_75 -= health /4;
        health_50 -= health /2;
        health_25 = health / 4;
        baseAttackSpeed = GetAttackRatePerSecond();


        hpSlider = transform.Find("Canvas/BossHealth/Boarder/HpBackground/BossHpBar").GetComponent<RectTransform>();

    }
    private void OnEnable()
    {
        // Kick off the repeating attack loop
        isDead = false;
        GameManager.instance.BossSpawned();
        attackRoutine = StartCoroutine(AttackLoop());
    }

    private void OnDisable()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
    }


    private IEnumerator AttackLoop()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(baseAttackSpeed);

            var phase = PickPhase();
            switch (phase)
            {
                case BossPhase.ChargeAttack:
                    StartCoroutine(Charge());
                    break;
                case BossPhase.Summon:
                    StartCoroutine(Summon());
                    break;
            }
        }
    }


    private void Update()
    {
        CheckHealth();
        float targetScale = (float)health / (float)_maxHealth; // Smooth HpBar change
        Vector3 currentScale = hpSlider.localScale;
        if (targetScale != currentScale.y)
        {
            float maxChangeThisFrame = .5f * Time.deltaTime; // if 100 FPS and maxChangepSec=0.5, this is 0.005

            if (Mathf.Abs(currentScale.y - targetScale) < maxChangeThisFrame)
            {
                currentScale.y = targetScale;
            }
            else
            {
                currentScale.y += Mathf.Sign(targetScale - currentScale.y) * maxChangeThisFrame;
            }

            hpSlider.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z); // End of smooth HpBar change
            Debug.Log("Damage changing");
        }
    }

    /// <summary>
    /// Just dash past the player. 10 units
    /// </summary>
    private IEnumerator Charge()
    {
        animator.SetBool("Charging", true);

        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;

        Vector3 direction = (playerPos - transform.position).normalized;
        if (direction.x < 0)
            rotateMeshHolder.localRotation = Quaternion.Euler(0, 0, 0);
        else
            rotateMeshHolder.localRotation = Quaternion.Euler(0, 180, 0);

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 targetPos = transform.position + direction * (distance + 15f);

        float elapsed = 0f, duration = GetMovementSpeed() / 5;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        animator.SetBool("Charging", false);
    }

    IEnumerator Summon()
    {
        animator.SetTrigger("Summon");
        yield return new WaitForSeconds(2.9f);

        currentPhase = BossPhase.Summon;
        for (int i = 0; i < spawnAliesCout; i++)
        {
            EnemyPrefab enemyToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count - 1)];

            float angle = Random.Range(0, Mathf.PI * Random.Range(2, 3));

            Vector3 rndPos = new Vector3(
                transform.position.x + Random.Range(10, 30) * Mathf.Cos(angle),
                0,
                transform.position.z + Random.Range(10, 30) * Mathf.Sin(angle)
            );

            GameObject enemy = Pooler.instance.SpawnFromPool(enemyToSpawn.enemyName, rndPos, Quaternion.identity);
            enemy.GetComponent<EnemyHealth_SYS>().IsBossSpawn();// will not interact with spawner.
                                                                //Spawner_var2.instance._enemiesOnScreen.Add(enemy); // ISSUE
            Debug.Log("spawning");
            yield return null;
        }
        
    }
    public void Enrage()
    {
        baseAttackSpeed *= 0.5f;
        
        SetMovementSpeed( GetMovementSpeed()/1.2f);
    }

    private void CheckHealth()
    {
        health = enemyhealth.GetEnemyCurrentHealth();

        if (isDead) return;

        // Death
        if (health <= 0)
        {
           StartCoroutine(Die());
            return;
        }
        if (health == health_75 || health == health_50 || health == health_25) Enrage();

    }

    IEnumerator Die()
    {
        isDead = true;
        animator.SetBool("Death", true);
        Debug.Log($"Boss died at health {health}");
        yield return new WaitForSeconds(3);
        GameManager.instance.ObBossDeth();
    }

    private BossPhase PickPhase()
    {
        float total = chargeWeight + summonWeight;
        float rand = Random.Range(0f, total);
        return rand < chargeWeight
            ? BossPhase.ChargeAttack
            : BossPhase.Summon;
    }



}
