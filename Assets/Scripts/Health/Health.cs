using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public class Health : IHealth
    {
        public int MaxHP { get; private set; }
        private int _hp;
        public event Action OnDeath = delegate { };

        public Health(int maxHp)
        {
            MaxHP = maxHp;
            _hp = maxHp;
        }

        public void TakeDamage(int damagePoints)
        {
            if (damagePoints < 0) return;
            _hp = Mathf.Clamp(_hp - damagePoints, 0, MaxHP);
            if (_hp <= 0) OnDeath?.Invoke();
        }

        public void Heal(int healPoints)
        {
            if (healPoints < 0) return;
            _hp = Mathf.Min(_hp + healPoints, MaxHP);
        }

        public int GetCurrentHealth() => _hp;
        public int GetMaxHealth() => MaxHP;
    }
}
