using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class UpgradeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Button> button = new List<Button>();
    private UnityAction[] _upgradeMethods;



    private List<Button> _existingButtons = new List<Button>();
    private PlayerStats _playerStats;
    //public delegate void UpgradeButtonDelegate();
    //public UpgradeButtonDelegate Upgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        _playerStats = PlayerManager.instance.playerStats;
        PlayerManager.instance.SetUpgradeRef(this);
        _upgradeMethods = new UnityAction[] {Upgrade0, Upgrade1 };
        OnLevelUp();
    }


    /// <summary>
    /// Call this whenever we level Up
    /// </summary>
    public void OnLevelUp() 
    {// Max 6 lvl's
        HashSet<int> usedIndexes = new HashSet<int>();
        if (button.Count < 2)
        {
            Debug.LogError("Not enough buttons to select 2 unique upgrades.");
            return;
        }
        for (int i = 0; i < 2;) 
        {
            int index = Random.Range(0, button.Count);
            if (!usedIndexes.Contains(index))
            {
                usedIndexes.Add(index);
                InstantiateButton(index);
                Debug.Log("nomegwebubgew");
                i++;
            }
        }
    }

    private void InstantiateButton(int index)
    {
        Button actionButton = Instantiate(button[index], transform, this.transform);
        actionButton.onClick.AddListener(_upgradeMethods[index]);
        actionButton.onClick.AddListener(DestroyExistingButtons);
        _existingButtons.Add(actionButton);
    }

    private void DestroyExistingButtons()
    {
        foreach (Button button in _existingButtons)
            Destroy(button.gameObject);
        _existingButtons.Clear();
    }

    public void Upgrade0()
    {//Increase of firerate
        float recoilSpeed = _playerStats.GetFireRate();
        _playerStats.IncreaseRecoilSpeed(10);
        Debug.Log($"Recoilspeed was {recoilSpeed}, now it is {_playerStats.GetFireRate()}");
    }

    public void Upgrade1()
    {//Increase the damage of the bullets
        float damage = _playerStats.GetBulletDamage();
        _playerStats.IncreaseDamage(10);
        Debug.Log($"Damage was {damage}, now it is {_playerStats.GetBulletDamage()}");
    }
}
