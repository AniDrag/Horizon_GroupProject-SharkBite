using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("===== Weapon Refrences =====")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform orientation;

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerStats playerStats;
    private float _timeReset;
    Vector3 _saveShoodDirection;
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerStats = PlayerManager.instance.playerStats;
        if (projectilePrefab.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Bullet doesn't have RigidBody");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timeReset += Time.deltaTime;

        Vector2 _moveDirection = _playerInput.actions["Look"].ReadValue<Vector2>();
        Vector3 _3dMoveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        if (_3dMoveDirection != Vector3.zero)
        {
            _saveShoodDirection = _3dMoveDirection;
            orientation.rotation = Quaternion.LookRotation(_saveShoodDirection);
        }

    }
    private void FixedUpdate()
    {
        if (_timeReset >= playerStats.GetFireRate())
        {
            _timeReset = 0;
            // Debug.Log("I shot a bullet");
            Shoot(playerStats.GetFireRate(), orientation.position);
        }
    }

    private void Shoot(float cd, Vector3 moveDirection)
    {

        GameObject newBullet = Instantiate(projectilePrefab, orientation.position + orientation.forward * 1.5f, Quaternion.identity);
        newBullet.GetComponent<Damage>().SetDamage(playerStats.GetBulletDamage());

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(orientation.forward * playerStats.GetBulletSpeed() * 200, ForceMode.Force);
        }
        Debug.Log("I shot a bullet");
    }
}
