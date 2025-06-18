using TMPro;
using UnityEngine;

public class LevelUp_SYS : MonoBehaviour
{

    public int level = 0;
    [SerializeField] int xpToLevelUp = 10;
    [SerializeField] TMP_Text levelUpText;

    [Header("========= Refrencec =========")]
    [SerializeField] UpgradeSystem upgradeSystem;
    private int _currentXP;

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
        upgradeSystem.OnLevelUp();
        levelUpText.text = level.ToString();
        Debug.Log($"player leveld up!! current level = {level}");
    }

}
