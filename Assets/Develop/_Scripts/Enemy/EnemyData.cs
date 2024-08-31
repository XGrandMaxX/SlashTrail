using UnityEngine;

namespace Develop._Scripts.Enemy
{
    [CreateAssetMenu(fileName = "NewEnemyData")]
    public class EnemyData : ScriptableObject
    {
        public int Health;
        
        [Space(10)]
        
        public float MoveSpeed;
        public float ChaseSpeed;
        
        [Space(10)]
        
        public float AgroRadius;
        public float AttackRange;
    }
}