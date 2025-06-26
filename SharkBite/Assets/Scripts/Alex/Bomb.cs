using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using static Unity.Collections.AllocatorManager;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 5;
    [SerializeField] private int damageToEnemy = 5;
    [SerializeField] private float bombRadius;
    [SerializeField] private LayerMask mask;
    [SerializeField] private bool showRadius;
    [SerializeField] private float timeToExplode = 2;
    [SerializeField] private float minFreq = 1f;   // hits per second at start
    [SerializeField] private float maxFreq = 20f;   // hits per second at the end
    [SerializeField] private Material sharedMat;


    bool _triggered;
    MeshRenderer mr;
    MaterialPropertyBlock block;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
        mr.sharedMaterial = sharedMat;
    }
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
            StartCoroutine(Blow());
        }
    }

    private IEnumerator Blow()
    {
        float startTime = Time.time;
        float endTime = startTime + timeToExplode;


        while (Time.time < endTime)
        {
            // 1) normalized progress 0→1
            float raw = (Time.time - startTime) / timeToExplode;
            raw = Mathf.Clamp01(raw);

            float envelope = Mathf.Pow(raw, 2f);

            float freq = Mathf.Lerp(minFreq, maxFreq, raw);

            // 4) oscillation around [0,1], at our changing frequency
            //    Time.time-startTime gives elapsed seconds; multiply by 2π for full cycles
            float osc = Mathf.Sin((Time.time - startTime) * freq * Mathf.PI * 2f);
            float flash = (osc + 1f) * 0.5f; //[−1,1] → [0,1]

            // 5) combine envelope with flash so early flashes are small,
            //    later flashes hit full red
            float t = flash * envelope;

            // 6) apply to shader
            mr.GetPropertyBlock(block);
            block.SetFloat("_Factor", t);
            mr.SetPropertyBlock(block);

            yield return null;
        }

        // final state: fully blown (solid red)
        mr.GetPropertyBlock(block);
        block.SetFloat("_Factor", 1f);
        mr.SetPropertyBlock(block);

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
        yield return null;
        Destroy(this.gameObject);
    }
}
