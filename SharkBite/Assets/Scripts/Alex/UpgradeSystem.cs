using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[Serializable]
public enum UpgradeType
{
    BulletSpeed,
    Firerate,
    Damage,
    MaxHealth,
    Defense,
    PlayerSpeed
}

[Serializable]
public class UpgradeOption
{
    [Tooltip("Name shown on the button")]
    public string displayName;

    [Tooltip("Which stat this upgrades")]
    public UpgradeType type;

    [Tooltip("How much to increase in percentage")]
    public float increasePercentage;

    [Tooltip("What is my sprite")]
    public Sprite spriteToDisplay;

    [Tooltip("What is my sprite")]
    public Sprite upgradeSpriteToDisplay;
}

[Serializable]
public struct UpgradeCondition
{

    [Tooltip("Which stat")]
    public UpgradeType type;

    [Tooltip("What level")]
    public int requiredLevel;
}

//[Serializable]
//public struct ButtonSprites
//{
//    public UpgradeType type;
//    public Sprite sprite;
//}

[Serializable]
public class BigUpgrade
{

    [Tooltip("Name shown on the button")]
    public string displayName;

    [Tooltip("What are the conditions")]
    public List<UpgradeCondition> conditions = new List<UpgradeCondition>();
    public UnityEvent onUnlocked;

    [Tooltip("This shouldn't repeat with other big upgrade id's")]
    public string id;

    [Tooltip("What is my sprite")]
    public Sprite spriteToDisplay;

    [Tooltip("What is my sprite")]
    public Sprite upgradeSpriteToDisplay;
}



public class UpgradeSystem : MonoBehaviour
{
    [Header("========== Configurable Upgrades ==========")]
    [Tooltip("Define all the upgrades you can ever pick")]
    [SerializeField] private List<UpgradeOption> basicUpgrades = new List<UpgradeOption>();

    [Tooltip("Max level for each basic upgrade")]
    [SerializeField] private int maxBasicLevel = 3;

    [Header("========== Big Upgrades ==========")]
    [SerializeField] private List<BigUpgrade> bigUpgrades = new List<BigUpgrade>();

    [Header("========== UI ==========")]
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private GameObject upgradeVisualPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Transform BasicUpgradeContainer;
    [SerializeField] private Transform UberUpgradeContainer;
    [Tooltip("How many choices to show on each level up")]
    [SerializeField] private int choicesPerLevel = 2;
    [Tooltip("Percentage bonus to apply once all upgrades are done")]
    [SerializeField] private float baseBonusPercent = 5f;

    //[Header("========== Button sprites ==========")]
    //[SerializeField] private List<ButtonSprites> buttonSprites = new List<ButtonSprites>(); // Don't use this, use the dictionary
    //private Dictionary<UpgradeType, Sprite> _buttonSprites; // Used for faster search

    private PlayerStats _playerStats;
    private Manager_Sound _audio;
    private List<Button> _spawnedButtons = new List<Button>();

    // tracking
    private Dictionary<UpgradeType, int> _basicLevels;
    private HashSet<string> _unlockedBig = new HashSet<string>();
    private Queue<BigUpgrade> _vipQueue = new Queue<BigUpgrade>();
    private HashSet<string> _queuedBig = new HashSet<string>();

    // inverted index: maps a (type, level) -> list of big upgrades that depend on it
    private Dictionary<UpgradeType, Dictionary<int, List<BigUpgrade>>> _conditionMap;

    /// <summary>
    /// I did this shit sorry
    /// </summary>
    private Dictionary<UpgradeType, (Image icon, TMP_Text counter)> _upgradeVisuals;
    private Dictionary<string, (Image icon, TextMeshProUGUI counter)> _bigUpgradeVisuals;

    private void Start()
    {
        if (choicesPerLevel > basicUpgrades.Count)
            Debug.LogWarning($"Choices per level {choicesPerLevel} are more than the upgrades list {basicUpgrades.Count}");

        _playerStats = PlayerManager.instance.playerStats;
        PlayerManager.instance.SetUpgradeRef(this);
        //Set basic levels for tracking
        _basicLevels = basicUpgrades.ToDictionary(u => u.type, u => 0);

        _upgradeVisuals = new Dictionary<UpgradeType, (Image icon, TMP_Text counter)>();
        _bigUpgradeVisuals = new Dictionary<string, (Image icon, TextMeshProUGUI counter)>();
        // FOR NICK
        // build inverted index for fast lookups
        // Example data:
        // bigUpgrades contains:
        //   BigUpgrade id="triple_shot", conditions=[{ type=Damage, requiredLevel=3 }, { type=Speed, requiredLevel=2 }]
        //   BigUpgrade id="platoon",     conditions=[{ type=FireRate, requiredLevel=3 }, { type=Damage, requiredLevel=2 }]
        _conditionMap = new Dictionary<UpgradeType, Dictionary<int, List<BigUpgrade>>>();
        foreach (var big in bigUpgrades)
        {
            foreach (var cond in big.conditions)
            {
                // cond.type might be Damage, cond.requiredLevel might be 3
                if (!_conditionMap.TryGetValue(cond.type, out var levelMap))
                {
                    // First time we see this stat type, create new inner map
                    // Before: _conditionMap does not have key "Damage"
                    levelMap = new Dictionary<int, List<BigUpgrade>>();
                    _conditionMap[cond.type] = levelMap;
                    // Now: _conditionMap[Damage] => empty dict
                }
                if (!levelMap.TryGetValue(cond.requiredLevel, out var upgradesAtLevel))
                {
                    // First time we see level 3 for Damage, create new list
                    // Before: levelMap for Damage does not have key 3
                    upgradesAtLevel = new List<BigUpgrade>();
                    levelMap[cond.requiredLevel] = upgradesAtLevel;
                    // Now: _conditionMap[Damage][3] => empty list
                }
                // Add this big upgrade to that list
                upgradesAtLevel.Add(big);
                // After: _conditionMap[Damage][3] contains big (e.g. triple_shot)
            }
        }

        // Inverted index now looks like:
        // {
        //   Damage    => { 3 => [triple_shot],    2 => [platoon] },
        //   Speed     => { 2 => [triple_shot] },
        //   FireRate  => { 3 => [platoon] }
        // }

        //_buttonSprites = buttonSprites.ToDictionary(bs => bs.type, bs => bs.sprite);

        OnLevelUp();
    }

    /// <summary>
    /// Call this when the player levels up
    /// </summary>
    public void OnLevelUp()
    {
        Time.timeScale = 0f;

        _audio?.PlaySFX(_audio.playerLevelUpSound);
        // if a VIP big upgrade is queued, show it instead
        if (_vipQueue.Count > 0)
        {
            SpawnBig(_vipQueue.Dequeue());
            return;
        }


        // Shuffle and take the first N
        var available = basicUpgrades.Where(u => _basicLevels[u.type] < maxBasicLevel).ToList();

        for (int i = 0; i < available.Count; i++)
        {
            var r = UnityEngine.Random.Range(i, available.Count);
            var tmp = available[i];
            available[i] = available[r];
            available[r] = tmp;
        }

        for (int i = 0; i < Mathf.Min(choicesPerLevel, available.Count); i++)
            SpawnButton(available[i]);


        if (available.Count == 0 && _vipQueue.Count == 0)
                ApplyBaseStatsBonus();

    }

    private void SpawnButton(UpgradeOption opt)
    {
        var btn = Instantiate(buttonPrefab, buttonContainer);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{opt.displayName} + {opt.increasePercentage}% ";
        btn.onClick.AddListener(() => ApplyUpgrade(opt));
        btn.onClick.AddListener(ClearExistingButtons);
        btn.GetComponent<UnityEngine.UI.Image>().sprite = opt.spriteToDisplay;
        _spawnedButtons.Add(btn);
    }

    private void ClearExistingButtons()
    {
        foreach (var b in _spawnedButtons)
            if (b) Destroy(b.gameObject);
        _spawnedButtons.Clear();
        Time.timeScale = 1f;
    }

    private void SpawnBig(BigUpgrade big)
    {
        var btn = Instantiate(buttonPrefab, buttonContainer);
        var label = btn.GetComponentInChildren<TextMeshProUGUI>().text = big.displayName;
        btn.onClick.AddListener(() => ApplyBig(big));
        btn.onClick.AddListener(ClearExistingButtons);
        btn.GetComponent<UnityEngine.UI.Image>().sprite = big.spriteToDisplay;
        _spawnedButtons.Add(btn);
    }
    private void ApplyBig(BigUpgrade big)
    {
        if (_unlockedBig.Contains(big.id)) return;
        _unlockedBig.Add(big.id);
        big.onUnlocked.Invoke(); // Invoke Unity event

        if (!_bigUpgradeVisuals.ContainsKey(big.id))
        {
            var go = Instantiate(upgradeVisualPrefab, UberUpgradeContainer);
            var img = go.GetComponentInChildren<Image>();
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();

            img.sprite = big.upgradeSpriteToDisplay;
            txt.text = "";

            _bigUpgradeVisuals.Add(big.id, (img, txt));
        }
    }

    private void ApplyUpgrade(UpgradeOption opt)
    {
        switch (opt.type)
        {
            case UpgradeType.BulletSpeed:
                _playerStats.IncreaseBulletSpeed(opt.increasePercentage);
                break;
            case UpgradeType.Firerate:
                _playerStats.IncreaseRecoilSpeed(opt.increasePercentage);
                break;
            case UpgradeType.Damage:
                _playerStats.IncreaseDamage(opt.increasePercentage);
                break;
            case UpgradeType.MaxHealth:
                _playerStats.IncreaseMaxHealth(opt.increasePercentage);
                break;
            case UpgradeType.Defense:
                _playerStats.IncreaseDefense(opt.increasePercentage);
                break;
            case UpgradeType.PlayerSpeed:
                _playerStats.IncreasePlayerSpeed(opt.increasePercentage);
                break;
        }
        _basicLevels[opt.type]++;
        int newLevel = _basicLevels[opt.type];

        if (_conditionMap.TryGetValue(opt.type, out var levelMap) && levelMap.TryGetValue(newLevel, out var bigList))
        {
            foreach (var big in bigList)
            {
                if (_queuedBig.Contains(big.id) || _unlockedBig.Contains(big.id))
                    continue;

                // verify all conditions met
                bool allMet = big.conditions.All(c => _basicLevels[c.type] >= c.requiredLevel);
                if (allMet)
                {
                    _vipQueue.Enqueue(big);
                    _queuedBig.Add(big.id);
                }
            }
        }
        if (!_upgradeVisuals.ContainsKey(opt.type))
        {
            var go = Instantiate(upgradeVisualPrefab, BasicUpgradeContainer);
            var img = go.GetComponentInChildren<Image>();
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();

            img.sprite = opt.upgradeSpriteToDisplay;
            txt.text = "";

            _upgradeVisuals.Add(opt.type, (img, txt));
        }
        else
        {
            var (img, txt) = _upgradeVisuals[opt.type];
            txt.text = $"{_basicLevels[opt.type]}x";
        }
    }

    private void ApplyBaseStatsBonus()
    {
        // small permanent boost when everything is done
        _playerStats.IncreaseBulletSpeed(baseBonusPercent);
        _playerStats.IncreaseDamage(baseBonusPercent);
        _playerStats.IncreaseRecoilSpeed(baseBonusPercent);
        _playerStats.IncreaseMaxHealth(baseBonusPercent);
        _playerStats.IncreaseDefense(baseBonusPercent);
        Time.timeScale = 1f;
    }

}
