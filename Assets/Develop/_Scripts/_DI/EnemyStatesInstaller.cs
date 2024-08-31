using Develop._Scripts.Enemy;
using DI;
using UnityEngine;

namespace Develop._Scripts._DI
{
    public class EnemyStatesInstaller : SceneContext
    {
        [SerializeField] private EnemyMove _enemyMove;
        [SerializeField] private EnemyAttack _enemyAttack;
        public override void RegisterDependencies()
        {
            RegisterMonoBehavior(_enemyMove, false);
            RegisterMonoBehavior(_enemyAttack, false);
        }
    }
}
