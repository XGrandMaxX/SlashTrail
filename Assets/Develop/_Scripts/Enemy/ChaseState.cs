using UnityEngine;

namespace Develop._Scripts.Enemy
{
    public sealed class ChaseState : IEnemyState
    {
        public void EnterState(EnemyStateMachine enemy)
        {
            Debug.Log("Entering chase state");
        }

        public void UpdateState(EnemyStateMachine enemy)
        {
            enemy.ChaseTarget();
            
            if (enemy.TargetInAttackSight())
            {
                enemy.TransitionToState(enemy._attackState);
                return;
            }

            if (!enemy.TargetInSight())
            {
                enemy.TransitionToState(enemy._patrolState);
                enemy.OnLoseSight?.Invoke();
            }
        }

        public void ExitState(EnemyStateMachine enemy)
        {
            Debug.Log("Exiting chase state");
        }
    }
}