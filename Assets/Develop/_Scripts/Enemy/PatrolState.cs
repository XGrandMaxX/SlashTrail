using UnityEngine;

namespace Develop._Scripts.Enemy
{
    public sealed class PatrolState : IEnemyState
    {
        public void EnterState(EnemyStateMachine enemy)
        {
            Debug.Log("Entering Patrol State");
        }

        public void UpdateState(EnemyStateMachine enemy)
        {
            enemy.Patrol();

            if (enemy.TargetInSight())
            {
                enemy.TransitionToState(enemy._chaseState);
            }
        }

        public void ExitState(EnemyStateMachine enemy)
        {
            Debug.Log("Exiting Patrol State");
        }
    }
}