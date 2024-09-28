using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public delegate void HealthPointsChangedEvent(int before, int after);

    public class Health
    {
        public int MaxHP { get; private set; }
        private int _hp;
        public event Action OnDeath = delegate { };
        public event HealthPointsChangedEvent OnDamage = delegate { };
        public event HealthPointsChangedEvent OnHeal = delegate { };

        public int HP
        {
            get => _hp;
            set => _hp = Mathf.Clamp(value, 0, MaxHP);
        }


        public Health(int maxHp)
        {
            MaxHP = maxHp;
            HP = maxHp;
        }
        public void TakeDamage(int damagePoints)
        {
            if (damagePoints < 0)
            {
                Debug.LogError("Damage cannot be less than 0");
            }

            var oldValue = HP;
            HP -= damagePoints;
            OnDamage?.Invoke(oldValue, HP);
            if (HP <= 0) 
            { 
            OnDeath?.Invoke();
            }
        }

        public void Heal(int healPoints)
        {
            if (healPoints < 0)
            {
                throw new ArgumentException("healPoints cannot be less than 0.");
            }

            var oldValue = HP;
            HP = Mathf.Min(HP + healPoints, MaxHP);
            OnHeal?.Invoke(oldValue, HP);
        }
    }
}
