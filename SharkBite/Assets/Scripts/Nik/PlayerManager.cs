using TMPro;
using UnityEngine;
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerHealth_SYS))]
[RequireComponent(typeof(Weapon))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerStats playerStats = new PlayerStats();
    public UpgradeSystem upgradeSystem;

    [Header("========= Refrencec =========")]
    [SerializeField] TMP_Text levelUpText;
    //private List<> modifiers = new List<>();


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
    }

    private void Start()
    {
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
