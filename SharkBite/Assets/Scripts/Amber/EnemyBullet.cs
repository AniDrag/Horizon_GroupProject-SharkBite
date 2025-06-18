using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float bulletForce = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletForce * Time.deltaTime;
    }
}
