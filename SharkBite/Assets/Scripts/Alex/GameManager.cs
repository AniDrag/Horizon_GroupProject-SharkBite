using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    public Vector3 _playerPos;
    public Vector2Int _displayResolution;

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
        _displayResolution = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
