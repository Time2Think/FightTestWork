using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayerController : MonoBehaviour
    {
        //movement fields
        [SerializeField]
        private Camera _playerCamera;
        [SerializeField]
        private float _movementForce = 1f;
        [SerializeField]
        private float _jumpForce = 5f;
        [SerializeField]
        private float _maxSpeed = 5f;
        private Vector3 _forceDirection = Vector3.zero;
        
        private Animator _animator;
        private Rigidbody _rb;
        //input fields
        private PlayerInputAction _playerActions;
        private InputAction _move;

        private void Awake()
        {
            _playerActions = new PlayerInputAction();
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _playerActions.Player.Jump.started += DoJump;
            _playerActions.Player.Attack.started += DoAttack;
            _playerActions.Player.DoubleAttack.started += DoDoubleAttack;
            _move = _playerActions.Player.Move;
            _playerActions.Player.Enable();
        }

        private void OnDisable()
        {
            _playerActions.Player.Jump.started -= DoJump;
            _playerActions.Player.Attack.started -= DoAttack;
            _playerActions.Player.DoubleAttack.started -= DoDoubleAttack;
            _playerActions.Player.Disable();
        }

        private void FixedUpdate()
        {
            _forceDirection += _move.ReadValue<Vector2>().x * GetCameraRight(_playerCamera) * _movementForce;
            _forceDirection += _move.ReadValue<Vector2>().y * GetCameraForward(_playerCamera) * _movementForce;

            _rb.AddForce(_forceDirection, ForceMode.Impulse);
            _forceDirection = Vector3.zero;

            if (_rb.velocity.y < 0f)
            {
                _rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime; 
            }
            Vector3 horizontalVelocity = _rb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > _maxSpeed * _maxSpeed)
            {
                _rb.velocity = horizontalVelocity.normalized * _maxSpeed + Vector3.up * _rb.velocity.y;
            }
            LookAt();
        }

        private void LookAt()
        {
            Vector3 direction = _rb.velocity;
            direction.y = 0f;

            if (_move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            {
                _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            else
            {
                _rb.angularVelocity = Vector3.zero;
            }
        }

        private Vector3 GetCameraForward(Camera playerCamera)
        {
            Vector3 forward = playerCamera.transform.forward;
            forward.y = 0;
            return forward.normalized;
        }

        private Vector3 GetCameraRight(Camera playerCamera)
        {
            Vector3 right = playerCamera.transform.right;
            right.y = 0;
            return right.normalized;
        }

        private void DoJump(InputAction.CallbackContext obj)
        {
            if(IsGrounded())
            {
                _forceDirection += Vector3.up * _jumpForce;
            }
        }

        private bool IsGrounded()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
            Debug.Log("RayHit = "+ Physics.Raycast(ray, out RaycastHit hi2t, 0.3f));
            if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            {return true;}
            else
            { return false;} 
        }

        private void DoAttack(InputAction.CallbackContext obj)
        {
            _animator.SetTrigger("attack");
        }
        private void DoDoubleAttack(InputAction.CallbackContext obj)
        {
            _animator.SetTrigger("duobleAttack");
        }
    }

