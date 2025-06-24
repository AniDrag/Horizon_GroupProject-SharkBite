using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    // ========= Geting Types =========
    private CharacterController _characterController;
    private PlayerStats playerStats;
    private PlayerInput _playerInput;


    // ========= Vectors =========
    private Vector3 _lastKnownDirection;


    public virtual void Start()
    {
        playerStats = GetComponent<PlayerManager>().playerStats;
        _characterController = GetComponent<CharacterController>();        
        _playerInput = GetComponent<PlayerInput>();
        _lastKnownDirection = transform.forward;
       

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

        _characterController.Move(_3dMoveDirection * playerStats.GetMovementSpeed() * Time.deltaTime);

        GameManager.instance._playerPos = transform.position;
    }
}
