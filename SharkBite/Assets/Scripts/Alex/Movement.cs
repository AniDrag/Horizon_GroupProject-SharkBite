using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private PlayerInput _playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (bulletPrefab.GetComponent<Rigidbody>() == null )
        {
            Debug.LogError("Bullet doesn't have RigidBody");
        }
        _lastKnownDirection = transform.forward;
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 _moveDirection = _playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 _3dMoveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        if (_3dMoveDirection != Vector3.zero)
        {
            _lastKnownDirection = _3dMoveDirection;
        }

        _characterController.Move(_3dMoveDirection * speed * Time.deltaTime);


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
