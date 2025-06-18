using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] 
    private List<Button> button = new List<Button>();
    //public delegate void UpgradeButtonDelegate();
    //public UpgradeButtonDelegate Upgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Button actionButton = Instantiate(button,transform);
        //actionButton.onClick.AddListener(Upgrade1);
    }


    /// <summary>
    /// Call this whenever we level Up
    /// </summary>
    public void OnLevelUp() 
    {

    }

    public void Upgrade1()
    {
        Debug.Log("I WORK");
    }

    public void Upgrade2()
    {
        Debug.Log("I WORK 2");
    }
}
