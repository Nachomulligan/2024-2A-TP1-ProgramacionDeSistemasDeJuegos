using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealthPoints = 100;
        private Health health;
        public event Action OnDeath;
        private void Awake()
        {
            health = new Health(maxHealthPoints);
            health.OnDeath += HandleDeath;
            health.OnDamage += HandleDamage;
            health.OnHeal += HandleHeal;
        }

        private void OnDestroy()
        {
            health.OnDeath -= HandleDeath;
            health.OnDamage -= HandleDamage;
            health.OnHeal -= HandleHeal;
        }

        public void TakeDamage(int damage)
        {
            health.TakeDamage(damage);
        }

        public void Heal(int amount)
        {
            health.Heal(amount);
        }

        private void HandleDeath()
        {
            Debug.Log($"{gameObject.name} has died.");
            OnDeath?.Invoke();
        }

        private void HandleDamage(int before, int after)
        {
            Debug.Log($"{gameObject.name} took damage: {before} -> {after}");
        }

        private void HandleHeal(int before, int after)
        {
            Debug.Log($"{gameObject.name} was healed: {before} -> {after}");
        }

        public int GetCurrentHealth() => health.HP;
        public int GetMaxHealth() => health.MaxHP;
    }
}

