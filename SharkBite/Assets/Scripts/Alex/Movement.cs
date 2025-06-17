using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Shooting")]
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float bulletForce;

    [Header("Prefabs")]
    [SerializeField] GameObject bulletPrefab;

    private CharacterController _characterController;
    private float _lastShotTime;
    private Vector3 _lastKnownDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (bulletPrefab.GetComponent<Rigidbody>() == null )
        {
            Debug.LogError("Bullet doesn't have RigidBody");
        }
        _lastKnownDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if (moveDirection != Vector3.zero)
        {
            _lastKnownDirection = moveDirection;
        }

        _characterController.Move(moveDirection * speed * Time.deltaTime);


        if (Time.time >= _lastShotTime + cooldown)
        {
            Shoot(cooldown, _lastKnownDirection);
            _lastShotTime = Time.time;
        }

        GameManager.instance._playerPos = transform.position;
    }

    private void Shoot(float cd, Vector3 moveDirection)
    {

        GameObject newBullet = Instantiate(bulletPrefab, transform.position + moveDirection, Quaternion.identity);
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(moveDirection.normalized * bulletForce);
        }
    }
}
