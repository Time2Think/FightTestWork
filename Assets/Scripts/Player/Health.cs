using System;
using Scripts.Player;
using UnityEngine;

    public class Health : MonoBehaviour, IDamageable
    {
        public event Action<float> OnHealthChanged;
        
        [SerializeField]
        private float _maxHealth;

        private float _currentHealth;
        
        public float CurrentHealth => _currentHealth;

        public void InitHp()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth);
        }

        public bool TakeDamage(float damageValue)
        {
            _currentHealth -= damageValue;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth / _maxHealth);
            return _currentHealth <= 0;
        }
        
    }
