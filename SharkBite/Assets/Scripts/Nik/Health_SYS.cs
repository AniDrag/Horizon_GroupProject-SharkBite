using TMPro;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Health_SYS : MonoBehaviour
{
    public int maxHelath = 10;

    [SerializeField] private bool isEnemy;

    private int _health = 10;


    [SerializeField] private TMP_Text healthText;
    private void Start()
    {
        _health = maxHelath;
        TakeDamage(0);
    }
    public void TakeDamage(int damage)
    {
        int tempHelth = _health - DamageCalculationWithModifiers(damage);
        if (tempHelth <= 0)
        {
            _health = 0;
            if (isEnemy) 
            {
                Spawner.instance.SPAWN_enemysInScene.Remove(gameObject);
                Destroy(gameObject); 
            }
            else gameObject.SetActive(false);

        }
        else
        {
            _health = tempHelth;
        }

        if(healthText != null) healthText.text = _health.ToString();

    }
    /// <summary>
    /// All modifiers and stuff can be managed here
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    int DamageCalculationWithModifiers(int damage)
    {
        return damage;
    }
}
