using TMPro;
using UnityEngine;
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(PlayerHealth_SYS))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerStats playerStats;
    public UpgradeSystem upgradeSystem;

    [Header("========= Refrencec =========")]
    [SerializeField] TMP_Text levelUpText;
    //private List<> modifiers = new List<>();

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
        xpToLevelUp = (int)(xpToLevelUp * 1.2f);
        upgradeSystem.OnLevelUp();
        levelUpText.text = level.ToString();
        Debug.Log($"player leveld up!! current level = {level}");
    }

    
}
