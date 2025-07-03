using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    

    [Header("===== Details =====")]
    public GameObject DethPanel;
    public Transform Player;
    public Camera renderCamera;
    public Vector3 _playerPos;
    public Vector2Int _displayResolution;
    public Vector2Int _displaySystemResolution;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // Player = objectByTag
        DethPanel.SetActive(false);
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        _displaySystemResolution = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
        _displayResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        Debug.Log(_displayResolution.ToString());
        Debug.Log(_displaySystemResolution.ToString());

        Manager_Sound.instance.PlayMusic(Manager_Sound.instance.normalCombatMusic);
        
           
    }




    public void PlayerDied()
    {
        DethPanel.SetActive(true);
    }

    public void BossSpawned(EnemyHealth_SYS health)
    {
        Manager_Sound.instance.TransitionMusic(Manager_Sound.instance.bossMusic);
        //bossHealthBar.GetComponent<BossHealth>.AsignHelthSys(health);
    }
}
