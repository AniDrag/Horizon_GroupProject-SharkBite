using TMPro;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class PlayerHealth_SYS : MonoBehaviour
{
    [Header ("===== Hp Refrences =====")]
    [SerializeField] private RectTransform hpSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float maxScaleChangePerSecond = 0.5f;


    private int _currentHealth = 10;
    private int _maxHealth = 10;
    private int _defense = 4;
    private PlayerStats _playerStats;



    public void Start()
    {
        _playerStats = PlayerManager.instance.playerStats;
        _currentHealth = _playerStats.GetMaxHealth();
        TakeDamage(0);
        _playerStats.OnStatsChanged += UpdateMaxHealth;
    }

    public void Update()
    {
        float targetScale = (float)_currentHealth / (float)_playerStats.GetMaxHealth(); // Smooth HpBar change
        float currentScale = hpSlider.localScale.y;
        healthText.text = _currentHealth.ToString();
        if (targetScale != currentScale)
        {
            float maxChangeThisFrame = maxScaleChangePerSecond * Time.deltaTime; // if 100 FPS and maxChangepSec=0.5, this is 0.005

            if (Mathf.Abs(currentScale - targetScale) < maxChangeThisFrame)
            {
                currentScale = targetScale;
            }
            else
            {
                currentScale += Mathf.Sign(targetScale - currentScale) * maxChangeThisFrame;
            }

            hpSlider.localScale = new Vector3(1, currentScale, 1); // End of smooth HpBar change
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            return;
        
        int tempHelth = _currentHealth - DamageCalculationWithModifiers(damage);

        if (tempHelth <= 0)
        {
            _currentHealth = 0;
             gameObject.SetActive(false);
        }
        else
        {
            _currentHealth = tempHelth;
        }

        if(healthText != null) healthText.text = _currentHealth.ToString();

    }
    int DamageCalculationWithModifiers(int damage)
    {
        damage -= _defense;
        if (damage < 0)
        {
            damage = 0;
        }
        return damage;
    }

    void UpdateMaxHealth()
    {
        _maxHealth = _playerStats.GetMaxHealth();
        _currentHealth = _maxHealth;
    }
}
