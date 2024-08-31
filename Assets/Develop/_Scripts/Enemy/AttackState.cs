using UnityEngine;

namespace Develop._Scripts.Enemy
{
    public sealed class AttackState : IEnemyState
    {
        public void EnterState(EnemyStateMachine enemy)
        {
            Debug.Log("Entering attack state");
        }

        public void UpdateState(EnemyStateMachine enemy)
        {
            enemy._enemyAttack.Attack();

            if (!enemy.TargetInAttackSight())
            {
                enemy.TransitionToState(enemy._chaseState);
            }
        }

        public void ExitState(EnemyStateMachine enemy)
        {
            Debug.Log("Exiting attack state");
        }
    }
}
