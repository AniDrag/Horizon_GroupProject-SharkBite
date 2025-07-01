using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    
    private int _damage = 5;
   public bool _triggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDamage(int setDamageTo)
    {
        _damage = setDamageTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth_SYS playerHealth = other.gameObject.GetComponent<PlayerHealth_SYS>();
        
        if (playerHealth != null && !_triggered)
        {

            Manager_Sound audio = Manager_Sound.instance;
            audio.PlaySFX(audio.bulletHit);
            _triggered = true;
            playerHealth.TakeDamage(_damage);
            Debug.Log($"Damage delt {_damage} to target");
        }

        _triggered = false;
        this.gameObject.SetActive(false);
    }
}
