using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[Serializable]
public enum UpgradeType
{
    BulletSpeed,
    RecoilSpeed,
    Damage,
    MaxHealth,
    Defense
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
}

public class UpgradeSystem : MonoBehaviour
{
    [Header("Configurable Upgrades")]
    [Tooltip("Define all the upgrades you can ever pick")]
    [SerializeField] private List<UpgradeOption> allUpgrades = new List<UpgradeOption>();

    [Header("UI")]
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [Tooltip("How many choices to show on each level up")]
    [SerializeField] private int choicesPerLevel = 2;

    private PlayerStats _playerStats;
    private List<Button> _spawnedButtons = new List<Button>();

    private void Start()
    {
        if (choicesPerLevel < allUpgrades.Count)
            Debug.LogWarning("Choices per level are more than the upgrades list");

        _playerStats = PlayerManager.instance.playerStats;
        PlayerManager.instance.SetUpgradeRef(this);
        OnLevelUp();

    }

    /// <summary>
    /// Call this when the player levels up
    /// </summary>
    public void OnLevelUp()
    {
        Time.timeScale = 0f;
        ClearExistingButtons();

        // Shuffle and take the first N
        var picks = new List<UpgradeOption>(allUpgrades);
        for (int i = 0; i < picks.Count; i++)
        {
            var r = UnityEngine.Random.Range(i, picks.Count);
            var tmp = picks[i];
            picks[i] = picks[r];
            picks[r] = tmp;
        }

        for (int i = 0; i < Mathf.Min(choicesPerLevel, picks.Count); i++)
            SpawnButton(picks[i]);
    }

    private void SpawnButton(UpgradeOption opt)
    {
        var btn = Instantiate(buttonPrefab, buttonContainer);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{opt.displayName} + {opt.increasePercentage}% ";
        btn.onClick.AddListener(() => ApplyUpgrade(opt));
        btn.onClick.AddListener(ClearExistingButtons);
        _spawnedButtons.Add(btn);
    }

    private void ClearExistingButtons()
    {
        foreach (var b in _spawnedButtons)
            if (b) Destroy(b.gameObject);
        _spawnedButtons.Clear();
        Time.timeScale = 1f;
    }

    private void ApplyUpgrade(UpgradeOption opt)
    {
        switch (opt.type)
        {
            case UpgradeType.BulletSpeed:
                _playerStats.IncreaseBulletSpeed(opt.increasePercentage);
                break;
            case UpgradeType.RecoilSpeed:
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
        }
    }
}
