using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Systems
{
    /// <summary>
    ///     Tracks and manages an entity's health, supports damage/healing operations,
    ///     and raises events when health changes or reaches zero.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth = 5;
        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            AudioManager.Instance?.PlayHit();
            if (_currentHealth <= 0) OnDeath?.Invoke();
        }

        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        public void Heal(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void ResetHealth()
        {
            _currentHealth = maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }
    }
}