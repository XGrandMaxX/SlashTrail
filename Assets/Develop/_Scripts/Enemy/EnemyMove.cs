using System;
using UnityEngine;

namespace Develop._Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class EnemyMove : Enemy
    {
        [field: Header("Patrol")]
        [field: SerializeField] internal float MoveSpeed { get; private set; }
        [field: SerializeField] internal Transform[] MovePoints { get; private set; }
        
        [field: Header("Chase")]
        [field: SerializeField] internal float ChaseSpeed { get; private set; }
        [SerializeField] internal Transform Target;

        public void Patrol()
        {
            Debug.Log("<color=yellow>PATROL</color>");
        }

        public void Chase()
        {
            Debug.Log("<color=cian>CHASE</color>");
            Vector3 direction = (Target.position - transform.position).normalized;

            transform.position += direction * (ChaseSpeed * Time.deltaTime);
            
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }

        public void SetNewTarget(Transform newTarget) => Target = newTarget;
    }
}
