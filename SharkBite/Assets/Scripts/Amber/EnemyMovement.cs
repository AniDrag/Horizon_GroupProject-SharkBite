using UnityEngine;

public class EnemyMovement : MonoBehaviour, IPooledObject
{
    
    private float _speed = 5;
    private GameManager _gm;

    private float _curentRange;
    [SerializeField] private float _meleRange = 2;
    [SerializeField] private float _rangedUnitRange = 10;
    [SerializeField] private Transform orientation;
    private bool _isRanged;
    private float distance;
    private EnemyCore _core;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _core = GetComponent<EnemyCore>();
        orientation = _core.GetMyOrientation();
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
        orientation.LookAt(new Vector3(_gm._playerPos.x, orientation.position.y, _gm._playerPos.z));
        distance = Vector3.Distance(transform.position, _gm._playerPos);
        Mathf.Abs(distance);
        if ((_curentRange - 0.1f) < distance && distance < (_curentRange + 0.1f))
        {
            //Debug.Log("in distance");

            if (_isRanged)
            {
                //float direction = Mathf.Sign(Time.time);
                transform.position += orientation.right * Time.deltaTime * _speed;
            }

        }
        else if (distance > _curentRange)
        {
            //Debug.Log("Too far awaz");
            transform.position += orientation.forward * _speed * Time.deltaTime;
        }
        else if (distance < _curentRange)
        {
            //Debug.Log("Too close awaz");
            transform.position += orientation.forward * (-_speed) * Time.deltaTime;
        }
    }

    void RusherEnemyBehaviour()
    {
        transform.position += orientation.forward * _speed * Time.deltaTime;
    }
}
