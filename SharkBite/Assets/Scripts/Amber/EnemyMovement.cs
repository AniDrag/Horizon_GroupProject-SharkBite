using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float speed = 5;
    [SerializeField] GameObject bulletPrefab;
   //[SerializeField] public float bulletForce = 1000;

    private float _lastShotTime;
    private Transform _player;
    private GameManager _gm;


    public float bulletForce = 1000;
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


        if (Time.time >= _lastShotTime + cooldown)
        {
            _lastShotTime = Time.time;
            Shoot();
        }
    }
    void Shoot()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0,0,1), transform.rotation);
        Debug.Log("shooting");
    }
}
