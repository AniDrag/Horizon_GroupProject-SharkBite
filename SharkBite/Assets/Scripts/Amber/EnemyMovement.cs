using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    private float _speed = 5;
    private GameManager _gm;

    private int _curentRange;
    private int _meleRange = 1;
    private int _rangedUnitRange = 10;
    private bool _isRanged;
    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _speed = GetComponent<EnemyCore>().GetMovementSpeed();
        _isRanged = GetComponent<EnemyCore>().GetEnemyType();
        _curentRange = _isRanged ? _rangedUnitRange : _meleRange;
    }

    // Update is called once per frame
    void Update()
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
}
