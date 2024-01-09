using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class ThirdPersonPlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private float _movementForce = 10f;
        [SerializeField]
        private float _jumpForce = 10f;
        [SerializeField]
        private float _maxSpeed = 5f;
        private float lastAttackTime = 0;
        private Vector3 _forceDirection = Vector3.zero;
        private bool _isJumping;
        
        private Animator _animator;
        private Rigidbody _rb;
        
        private Camera _playerCamera;
        private InputAction _move;
        private PlayerInputAction _playerActions;
        private Cooldown _cooldown;
        private Ability _ability;
        
        [Inject]
        private void Construct (PlayerInputAction playerActions,Ability ability, Cooldown cooldown)
        {
            _playerActions = playerActions;
            _ability = ability;
            _cooldown = cooldown;
        }
        
        private void Awake()
        {
            _playerCamera = Camera.main;
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
        
        public void DeactiveInputControll()
        {
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
        
        private void DoAttack(InputAction.CallbackContext obj)
        {
            if (_cooldown.IsCoolDownAttack) return;
            _ability.StartAttackCooldown();
            _animator.SetTrigger("attack");
        }
        
        private void DoDoubleAttack(InputAction.CallbackContext obj)
        {
            if (_cooldown.IsCoolDownDoubleAttack) return;
            _ability.StartDoubleAttackCooldown();
            _animator.SetTrigger("doubleAttack");
        }
        
        private void Attack()
        {
            var enemies = SceneManager.Instance.Enemies;
            Enemie closestEnemie = null;

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemie = enemies[i];
                if (enemie == null)
                {
                    continue;
                }

                if (closestEnemie == null)
                {
                    closestEnemie = enemie;
                    continue;
                }

                var distance = Vector3.Distance(transform.position, enemie.transform.position);
                var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

                if (distance < closestDistance)
                {
                    closestEnemie = enemie;
                }

            }
            if (closestEnemie != null)
            {
                var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
                if (distance <= _player.Weapon.AttackRange)
                {
                    if (Time.time - lastAttackTime > _player.Weapon.AtackSpeed)
                    {
                        //transform.LookAt(closestEnemie.transform);
                        transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);
                        lastAttackTime = Time.time;
                        closestEnemie.Health.TakeDamage(_player.Weapon.Damage);
                    }
                }
            }
        }
        
        private void DoJump(InputAction.CallbackContext obj)
        {
            if (IsGrounded() && !_isJumping)
            {
                _isJumping = true;
                _animator.SetTrigger("jump");
                _forceDirection += Vector3.up * _jumpForce;
                _isJumping = false;
            }
        }
        
        private bool IsGrounded()
        {
            int groundLayerMask = LayerMask.GetMask("Ground");
            Vector3 rayOrigin = transform.TransformPoint(Vector3.up * 0.25f);
            Ray ray = new Ray(rayOrigin, Vector3.down);
            bool hitGround = Physics.Raycast(ray, out RaycastHit hit, 0.3f, groundLayerMask);
            return hitGround;
        }
    }
}

