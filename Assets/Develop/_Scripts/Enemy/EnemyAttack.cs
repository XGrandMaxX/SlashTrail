using System;
using UnityEngine;

namespace Develop._Scripts.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyAttack : Enemy
    {        
        public event Action OnAttack;
        [field: SerializeField] internal float AttackCooldown { get; private set; }
        internal float LastAttackTime { get; set; }
        
        [field: Space(10)]
        [field: SerializeField] internal float AgroRadius { get; private set; }
        [field: SerializeField] internal float AttackRange { get; private set; }
        
        public void Attack()
        {
            float elapsedTime = Time.time - LastAttackTime;
            
            if (elapsedTime >= AttackCooldown)
            {
                LastAttackTime = Time.time;
                
                Debug.Log("<color=red>ATTACK</color>");
                OnAttack?.Invoke();
            }
        }
    }
}
