using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("===== Weapon Refrences =====")]
    [SerializeField] private Transform orientation;

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Pooler itemPooler;
    Manager_Sound _audio;

    int _upgradelevel = 0;
    int bulletcount = 1;
    float angleStep = 10f; // degrees between bullets

    private float _timeReset;
    private Animator _animation;
    Vector3 _saveShoodDirection;
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerStats = PlayerManager.instance.playerStats;
        _audio = Manager_Sound.instance;
        itemPooler = Pooler.instance;
        _animation = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        

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
        _timeReset += Time.deltaTime;
        if (_timeReset >= playerStats.GetFireRate())
        {
            _timeReset = 0;

            Shoot(playerStats.GetFireRate(), orientation.position);
        }
    }

    private void Shoot(float cd, Vector3 moveDirection)
    {
        _audio.PlaySFX(_audio.playerShootSound);
        for (int i = 0; i < bulletcount; i++)
        {
            float startAngle = -(bulletcount - 1) * angleStep / 2f;
            float currentAngle = startAngle + i * angleStep;
            Quaternion bulletRotation = Quaternion.Euler(0, currentAngle, 0) * orientation.rotation;

            GameObject newBullet = itemPooler.SpawnFromPool("Bullet", orientation.position + orientation.forward * 1.5f, bulletRotation);
            newBullet.GetComponent<Damage>().SetDamage(playerStats.GetBulletDamage());

            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {                
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(newBullet.transform.forward * playerStats.GetBulletSpeed() * 300, ForceMode.Force);
            }
            //Debug.Log("I shot a bullet");
        }
    }

    public void TripleShot()
    {
        bulletcount = 3;
    }
}
