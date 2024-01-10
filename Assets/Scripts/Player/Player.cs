using UnityEngine;
using Weapons;
using Zenject;

    public class Player : MonoBehaviour
    {
        [SerializeField]
        public Animator _animator;
        [SerializeField]
        private ThirdPersonPlayerController _playerController;
        [SerializeField]
        private Weapon _weapon;
        [SerializeField]
        private Health _health;
        public Weapon Weapon => _weapon;
        public Health Health => _health;
        
        private bool _isDead;
        
        private BattleController _battleController;
        

        [Inject]
        private void Construct (BattleController battleController)
        {
            _battleController = battleController;
        }

        private void Awake()
        {
            _health.OnHealthChanged += CheckHealth;
        }

        private void Start()
        {
            Health.InitHp();
        }
        private void OnDestroy()
        {
            _health.OnHealthChanged -= CheckHealth;
        }
        
        private void CheckHealth(float fillAmount)
        {
            if (_isDead) return;
            if (fillAmount <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            _isDead = true;
            _playerController.DeactiveInputControll();
            _animator.SetTrigger("die");
            _battleController.GameOver();
        }
    }

