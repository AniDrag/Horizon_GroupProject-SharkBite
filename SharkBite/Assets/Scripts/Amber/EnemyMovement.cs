using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] GameObject BulletPrefab;


    private Transform _player;
    private GameManager _gm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_gm._playerPos);
        float distance = Vector2.Distance(transform.position, _gm._playerPos);
        if(distance > 1)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        
    }
}
