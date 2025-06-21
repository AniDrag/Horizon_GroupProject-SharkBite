using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    private float speed = 5;
    private GameManager _gm;

    private int curentRange;
    private int meleRange = 1;
    private int rangedUnitRange = 10;
    private bool isRanged;
    public float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        speed = GetComponent<EnemyCore>().GetMovementSpeed();
        isRanged = GetComponent<EnemyCore>().GetEnemyType();
        curentRange = isRanged ? rangedUnitRange : meleRange;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(_gm._playerPos.x, transform.position.y, _gm._playerPos.z));
        distance = Vector3.Distance(transform.position, _gm._playerPos);
        Mathf.Abs(distance);

        if ((curentRange - 0.1f) < distance && distance < (curentRange + 0.1f))
        {
            Debug.Log("in distance");

            if (isRanged)
            {
                //float direction = Mathf.Sign(Time.time);
                transform.position += transform.right * Time.deltaTime * speed;
            }

        }
        else if (distance > curentRange)
        {
            Debug.Log("Too far awaz");
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (distance < curentRange)
        {
            Debug.Log("Too close awaz");
            transform.position += transform.forward * (-speed) * Time.deltaTime;
        }
    }
}
