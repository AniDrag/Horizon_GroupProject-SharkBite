using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Collider))]
public class PlayerHealth_SYS : MonoBehaviour
{
    public bool GODMODE;

    [Header ("===== Hp Refrences =====")]
    [SerializeField] private RectTransform hpSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float maxScaleChangePerSecond = 0.5f;


    public int _currentHealth = 10;
    private int _maxHealth = 10;
    private int _defense = 4;
    [SerializeField] private PlayerStats _playerStats;
    Manager_Sound _audio;



    public void Start()
    {
        _playerStats = PlayerManager.instance.playerStats;
        _currentHealth = _playerStats.GetMaxHealth();
        //TakeDamage(0);
        _playerStats.OnStatsChanged += UpdateMaxHealth;
        _audio = Manager_Sound.instance;
    }

    public void Update()
    {
        float targetScale = (float)_currentHealth / (float)_playerStats.GetMaxHealth(); // Smooth HpBar change
        Vector3 currentScale = hpSlider.localScale;
        healthText.text = _currentHealth.ToString();
        if (targetScale != currentScale.y)
        {
            float maxChangeThisFrame = maxScaleChangePerSecond * Time.deltaTime; // if 100 FPS and maxChangepSec=0.5, this is 0.005

            if (Mathf.Abs(currentScale.y - targetScale) < maxChangeThisFrame)
            {
                currentScale.y = targetScale;
            }
            else
            {
                currentScale.y += Mathf.Sign(targetScale - currentScale.y) * maxChangeThisFrame;
            }

            hpSlider.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z); // End of smooth HpBar change
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0 || GODMODE)
            return;
        Debug.Log($"I took damage Player-{damage}");
        int tempHelth = _currentHealth - DamageCalculationWithModifiers(damage);
        _audio.PlaySFX(_audio.playerDamaged);

        if (tempHelth <= 0)
        {
            _currentHealth = 0;
            _audio.PlaySFX(_audio.playerDethSound);

            GameManager.instance.PlayerDied();
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
