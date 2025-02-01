using System;
using UnityEngine;

namespace Base_Classes
{
    public class Life : MonoBehaviour
    {
        public event Action onHealthChange;
        public event Action onDamageTaken;
        public event Action onIsDead;
    
        public int maxHealth;
        public int health;
    
        [HideInInspector]
        public bool isInvincible = false;
    
        public virtual void Start()
        {
            if(health == 0) health = maxHealth;
        }

        public virtual void AddHealth(int summand)
        {
            if(summand < 0)
            {
                TakeDamage(-summand);
                return;
            }

            health += summand;
            if (health > maxHealth) health = maxHealth;
            else onHealthChange?.Invoke();
        }

        public virtual void RestoreHealth()
        {
            health = maxHealth;
            onHealthChange?.Invoke();
        }

        public virtual void TakeDamage(int summand)
        {
            if(isInvincible || IsDead()) return;
            if(summand < 0)
            {
                AddHealth(-summand);
                return;
            }
        
            health -= summand;
            if (health + summand > 0 && health <= 0)
            {
                onHealthChange?.Invoke();
                onIsDead?.Invoke();
            }
            else if (health < 0)
            {
                health = 0;
            }
            else
            {
                onHealthChange?.Invoke();
                onDamageTaken?.Invoke();
            }
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    
        public void SetInvincible(bool invincible)
        {
            isInvincible = invincible;
        }
    
    }
}
