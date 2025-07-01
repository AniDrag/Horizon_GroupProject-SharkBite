using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using static Unity.Collections.AllocatorManager;
using System.Numerics;

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
    Material _radiusMat;
    MeshRenderer _radiusMr;
    MaterialPropertyBlock _radiusBlock;

    MeshRenderer _mr;
    MaterialPropertyBlock _block;



    private void Awake()
    {
        if (sharedMat != null)
        {
            _mr = GetComponent<MeshRenderer>();
            if (_mr == null)
            {
                Debug.LogWarning("I don't have a MeshRenderer");
                return;
            }
            _block = new MaterialPropertyBlock();
            Texture baseTex = _mr.sharedMaterial.GetTexture("_BaseMap");
            _block.SetTexture("_MyTexture", baseTex);

            _mr.sharedMaterial = sharedMat;
            _mr.SetPropertyBlock(_block);

            _radiusMr = transform.GetChild(0).GetComponent<MeshRenderer>();
            _radiusBlock = new MaterialPropertyBlock();
            _radiusMr.sharedMaterial = _radiusMr.material;
            _radiusMr.SetPropertyBlock(_radiusBlock);
            ApplyFactor(0);

        }
        else
            Debug.LogWarning("I don't have reference to the shader material");
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
            ApplyFactor(t);

            yield return null;
        }

        // final state: fully blown (solid red)
        ApplyFactor(1);
        Manager_Sound audio = Manager_Sound.instance;
        audio.PlaySFX(audio.barrelExplosion);

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
        Destroy(this.transform.parent.gameObject);
    }

    private void ApplyFactor(float factor)
    {
        _mr.GetPropertyBlock(_block);
        _block.SetFloat("_Factor", factor);
        _mr.SetPropertyBlock(_block);

        _radiusMr.GetPropertyBlock(_radiusBlock);
        _radiusBlock.SetFloat("_Alpha", factor);
        _radiusMr.SetPropertyBlock(_radiusBlock);
    }


}
