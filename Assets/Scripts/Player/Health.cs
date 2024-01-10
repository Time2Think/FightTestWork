using System;
using Scripts.Player;
using UnityEngine;

    public class Health : MonoBehaviour, IDamageable
    {
        public event Action<float, float> OnHealthChanged;
        
        [SerializeField]
        private float _maxHealth;

        private float _currentHealth;
        
        public float CurrentHealth => _currentHealth;

        public void InitHp()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth,0);
        }

        public bool TakeDamage(float damageValue)
        {
            _currentHealth -= damageValue;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth / _maxHealth,damageValue);
            return _currentHealth <= 0;
        }
        
        public bool TakeHeal(float healValue)
        {
            _currentHealth += healValue;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth / _maxHealth,healValue);
            return _currentHealth <= 0;
        }
        
    }
