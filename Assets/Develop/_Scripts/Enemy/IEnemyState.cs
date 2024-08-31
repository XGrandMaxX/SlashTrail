namespace Develop._Scripts.Enemy
{
    public interface IEnemyState
    {
        void EnterState(EnemyStateMachine enemy);
        void UpdateState(EnemyStateMachine enemy);
        void ExitState(EnemyStateMachine enemy);
    }
}
