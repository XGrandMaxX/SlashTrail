using System;
using UnityEngine;

namespace DI
{
    public abstract class ProjectContext : Context
    {
        private void Awake()
        {
            RegisterDependencies();
            _container.SetProjectDependencies(_registrations);
            _container.Initialize();
        }
    }
}