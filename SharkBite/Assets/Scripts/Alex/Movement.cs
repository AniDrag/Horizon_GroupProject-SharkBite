using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{

    // ========= Geting Types =========
    private CharacterController _characterController;
    private PlayerStats playerStats;
    private PlayerInput _playerInput;
    private Animator _animator;

    // ========= Vectors =========
    private Vector3 _lastKnownDirection;

    

    public virtual void Start()
    {
        playerStats = GetComponent<PlayerManager>().playerStats;
        _characterController = GetComponent<CharacterController>();        
        _playerInput = GetComponent<PlayerInput>();
        _lastKnownDirection = transform.forward;
        _animator = transform.GetChild(1).GetComponent<Animator>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        Vector2 _moveDirection = _playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 _3dMoveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        if (_3dMoveDirection != Vector3.zero)
        {
            _lastKnownDirection = _3dMoveDirection;
        }
        #region Animation logic
        bool isRunning = _moveDirection != Vector2.zero;
        _animator.SetBool("isRunning", isRunning);
        _animator.SetFloat("AxisX", _moveDirection.x);
        _animator.SetFloat("AxisZ", _moveDirection.y);
        #endregion

        _characterController.Move(_3dMoveDirection * playerStats.GetMovementSpeed()*2 * Time.deltaTime);

        GameManager.instance._playerPos = transform.position;
    }
}
