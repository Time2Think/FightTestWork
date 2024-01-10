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
        private Axe _weapon;
        [SerializeField]
        private Health _health;
        public Weapon Weapon => _weapon;
        public Health Health => _health;
        
        private bool _isDead;
        private float _startDamage;
        private BattleController _battleController;
        private float lastAttackTime = 0;

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
            _startDamage = Weapon.Damage;
        }
        
        private void Update()
        {
            if(_isDead)
            {
                return;
            }
            FindClosestEnemy();
        }

        private void FindClosestEnemy()
        {
            Enemy closestEnemy = null;

            for (int i = 0; i < _battleController.Enemies.Count; i++)
            {
                var enemie = _battleController.Enemies[i];
                if (enemie == null)
                {
                    continue;
                }

                if (closestEnemy == null)
                {
                    closestEnemy = enemie;
                    continue;
                }

                var distance = Vector3.Distance(transform.position, enemie.transform.position);
                var closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

                if (distance < closestDistance)
                {
                    closestEnemy = enemie;
                }

            }
            if (closestEnemy != null)
            {
                var distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                if (distance <= Weapon.AttackRange)
                {
                    if (Time.time - lastAttackTime > Weapon.AtackSpeed)
                    {
                        transform.transform.rotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
                        lastAttackTime = Time.time;
                        closestEnemy.Health.TakeDamage(Weapon.Damage);
                    }
                }
            }
        }
        private void OnDestroy()
        {
            _health.OnHealthChanged -= CheckHealth;
        }
        
        private void CheckHealth(float fillAmount, float damage)
        {
            if (_isDead) return;
            if (fillAmount <= 0)
            {
                Die();
            }
        }

        public void ActiveDoubleDamage()
        {
            _weapon.Damage *= 2;
        }
        public void ActiveNormalDamage()
        {
            _weapon.Damage = _startDamage;
        }
        private void Die()
        {
            _isDead = true;
            _playerController.DeactiveInputControll();
            _animator.SetTrigger("die");
            _battleController.GameOver();
        }
    }

