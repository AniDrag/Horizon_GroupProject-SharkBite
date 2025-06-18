using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class UpgradeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] 
    private List<Button> button = new List<Button>();
    private UnityAction[] upgradeMethods;



    private List<Button> _existingButtons = new List<Button>();
    //public delegate void UpgradeButtonDelegate();
    //public UpgradeButtonDelegate Upgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeMethods = new UnityAction[] {Upgrade0, Upgrade1 };
        OnLevelUp();
    }


    /// <summary>
    /// Call this whenever we level Up
    /// </summary>
    public void OnLevelUp() 
    {
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
        actionButton.onClick.AddListener(upgradeMethods[index]);
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
    {
        Debug.Log("I WORK");
    }

    public void Upgrade1()
    {
        Debug.Log("I WORK 2");
    }
}
