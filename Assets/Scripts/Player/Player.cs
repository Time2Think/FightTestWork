using UnityEngine;
using Weapons;

namespace Scripts.Player
{
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
            SceneManager.Instance.GameOver();
        }
    }
}
