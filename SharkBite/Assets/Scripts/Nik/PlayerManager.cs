using TMPro;
using UnityEngine;
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(PlayerHealth_SYS))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    [Range(1, 100)] int xpModifier;
    public static PlayerManager instance;
    public PlayerStats playerStats;
    public UpgradeSystem upgradeSystem;

    [Header("========= Refrencec =========")]
    [SerializeField] TMP_Text levelUpText;
    //private List<> modifiers = new List<>();

    [Header("===== Xp Refrences =====")]
    [SerializeField] private RectTransform xpSlider;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private float maxScaleChangePerSecond = 0.5f;

    // ========= Ints =========
    private int level = 0;
    private int xpToLevelUp;    
    private int _currentXP;

    public void SetUpgradeRef(UpgradeSystem transfer) => upgradeSystem = transfer;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
            instance = this;
        playerStats = new PlayerStats();
        xpToLevelUp = playerStats.GetMaxXP();
    }

    public void Update()
    {
        float targetScale = (float)_currentXP / (float)xpToLevelUp; // Smooth HpBar change
        Vector3 currentScale = xpSlider.localScale;
        xpText.text = level.ToString();
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

            xpSlider.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z); // End of smooth HpBar change
        }
    }

    public void AddXP(int xp)
    {
        int tempXP = xp + _currentXP;
        if (tempXP >= xpToLevelUp)
        {
            tempXP -= xpToLevelUp;
            LevelUP();

        }

        _currentXP = tempXP;

    }

    void LevelUP()
    {
        level++;
        xpToLevelUp = (int)(xpToLevelUp + xpModifier);
        if (level < 6)
        {
            upgradeSystem.OnLevelUp();
            levelUpText.text = level.ToString();
            Debug.Log($"player leveld up!! current level = {level}");
        }
        else
        {
            Debug.Log($"Players level is maxed out! Level = {level}");
            levelUpText.text = $"Max level: {level}";
        }
    }

    
}
