using UnityEngine;

public class Damage : MonoBehaviour
{
    private int _damage = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDamage(int setDamageTo)
    {
        _damage = setDamageTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health_SYS targetHelth = other.gameObject.GetComponent<Health_SYS>();
        if (targetHelth != null)
        {
            targetHelth.TakeDamage(_damage);
            Debug.Log($"Damage delt {_damage} to target");
        }
    }
}
