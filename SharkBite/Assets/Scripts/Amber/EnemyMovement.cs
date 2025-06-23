using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    private float _speed = 5;
    private GameManager _gm;

    private int _curentRange;
    [SerializeField] private int _meleRange = 2;
    [SerializeField] private int _rangedUnitRange = 10;
    private bool _isRanged;
    private float distance;
    private EnemyCore _core;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _core = GetComponent<EnemyCore>();
        _speed = _core.GetMovementSpeed();
        _isRanged = _core.GetEnemyType();
        _curentRange = _isRanged ? _rangedUnitRange : _meleRange;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_core.enemyType != EnemyCore.EnemyType.Rusher)
        {
            NormalEnemyBehaviour();
        }
        else
        {
            RusherEnemyBehaviour();
        }
        
    }
    void NormalEnemyBehaviour()
    {
        transform.LookAt(new Vector3(_gm._playerPos.x, transform.position.y, _gm._playerPos.z));
        distance = Vector3.Distance(transform.position, _gm._playerPos);
        Mathf.Abs(distance);
        if ((_curentRange - 0.1f) < distance && distance < (_curentRange + 0.1f))
        {
            //Debug.Log("in distance");

            if (_isRanged)
            {
                //float direction = Mathf.Sign(Time.time);
                transform.position += transform.right * Time.deltaTime * _speed;
            }

        }
        else if (distance > _curentRange)
        {
            //Debug.Log("Too far awaz");
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
        else if (distance < _curentRange)
        {
            //Debug.Log("Too close awaz");
            transform.position += transform.forward * (-_speed) * Time.deltaTime;
        }
    }

    void RusherEnemyBehaviour()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
}
