using System;

namespace DI
{
    public abstract class SceneContext : Context
    {
        private void Awake()
        {
            RegisterDependencies();
            _container.SetSceneDependencies(_registrations);
            _container.Initialize();
        }
    }
}