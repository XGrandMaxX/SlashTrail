using System;
using UnityEngine;

namespace Develop._Scripts.Enemy
{
    [RequireComponent(typeof(EnemyMove), typeof(EnemyAttack))]
    [DisallowMultipleComponent]
    public class EnemyStateMachine : MonoBehaviour
    {
        public Action OnDie;
        public Action OnBlock;
        public Action OnLoseSight;
        
        private IEnemyState _currentState;
        internal EnemyMove _enemyMove { get; private set; }
        internal EnemyAttack _enemyAttack { get; private set; }

        internal ChaseState _chaseState { get; private set; }
        internal PatrolState _patrolState { get; private set; }
        
        internal AttackState _attackState { get; private set; }

        private void Awake()
        {
            InitializeComponents();
            InitializeStates();
        }

        private void InitializeComponents()
        {
            _enemyMove = GetComponent<EnemyMove>();
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        private void InitializeStates()
        {
            _chaseState = new ChaseState();
            _patrolState = new PatrolState();
            _attackState = new AttackState();
        }

        private void Start() => TransitionToState(new PatrolState());

        private void Update() => _currentState.UpdateState(this);

        public void TransitionToState(IEnemyState newState)
        {
            _currentState?.ExitState(this);

            _currentState = newState;
            
            _currentState.EnterState(this);
        }

        public void Patrol() => _enemyMove.Patrol();

        public void ChaseTarget()
        {
            if (!TargetInSight())
            {
                return;
            }
            
            _enemyMove.Chase();
        }

        public bool TargetInSight() 
            => Vector3.Distance(transform.position, _enemyMove.Target.transform.position) < _enemyAttack.AgroRadius;

        public bool TargetInAttackSight() 
            => Vector3.Distance(transform.position, _enemyMove.Target.transform.position) < _enemyAttack.AttackRange;
    }
}
