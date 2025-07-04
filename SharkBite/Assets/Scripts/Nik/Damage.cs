using UnityEngine;

public class Damage : MonoBehaviour
{

    private int _damage = 5;
    bool _triggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDamage(int setDamageTo)
    {
        _damage = setDamageTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        EnemyHealth_SYS enemyhealth = other.gameObject.GetComponent<EnemyHealth_SYS>();
        if (enemyhealth !=null&& !_triggered)
        {

            Manager_Sound audio = Manager_Sound.instance;
            audio.PlaySFX(audio.bulletHit, 0.2f);
            _triggered = true;
            enemyhealth.TakeDamage(_damage);
            //Debug.Log($"Damage delt {_damage} to target");
        }
        //Debug.Log("triggererered");
        _triggered = false;
        this.gameObject.SetActive(false);
    }
}
