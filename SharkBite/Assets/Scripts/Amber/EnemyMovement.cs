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
    private int _direction;
    public Animator _animator;
    [SerializeField] Transform plazer;// delet this
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        _core = GetComponent<EnemyCore>();
        _animator = _core.GetAnimator();
       // _animator = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        _direction = Random.value < 0.5f ? -1 : 1;
        // _gm = GameObject.Find("GameManager").GetComponent<GameManager>(); Uncoment me

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


        orientation.LookAt(new Vector3(/*_gm._playerPos.x*/plazer.position.x, orientation.position.y, plazer.position.z)); //_gm._playerPos.z));
        distance = Vector3.Distance(transform.position, /*_gm._playerPos*/ plazer.position);
        Mathf.Abs(distance);
        if ((_curentRange - 0.1f) < distance && distance < (_curentRange + 0.1f))// player is within a range smaller than distance
        {
            //Debug.Log("in distance");
            _animator.SetBool("isRunning", false);
          //  Debug.Log("direction value" + direction);
            if (_isRanged)
            {
                //float direction = Mathf.Sign(Time.time);
                _animator.SetBool("isRunning", true);
                transform.position += orientation.right * _direction *Time.deltaTime * _speed;

            }

        }
        else if (distance > _curentRange)// distance bigger than currentrange
        {      
            _animator.SetBool("isRunning", true);
            Debug.Log("Too far awaz");
            transform.position += orientation.forward * _speed * Time.deltaTime;
        }
        else if (distance < _curentRange) // too close to player
        {
            _animator.SetBool("isRunning", false);
            //Debug.Log("Too close awaz");
            transform.position += orientation.forward * (-_speed) * Time.deltaTime;
        }
    }

    void RusherEnemyBehaviour()
    {
        transform.position += orientation.forward * _speed * Time.deltaTime;
    }
}
