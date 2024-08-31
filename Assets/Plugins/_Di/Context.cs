using System;
using System.Collections.Generic;
using System.Linq;
using _Di;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DI
{
    [DefaultExecutionOrder(-1000)]
    public abstract class Context : MonoBehaviour
    {
        public abstract void RegisterDependencies();

        protected DiContainer _container = DiContainer.Instance;

        public readonly Dictionary<Type, Registration> _registrations = new();

        

        public void Register<TService, TImplementation>(bool isSingleton) where TImplementation : TService
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => CreateInstance<TImplementation>(),
                IsSingleton = isSingleton
            };

            Debug.Log($"Registered {typeof(TService).FullName} as {(isSingleton ? "singleton" : "transient")}");
        }
        
        public void Register<TService>(bool isSingleton)
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => CreateInstance<TService>(),
                IsSingleton = isSingleton
            };

            Debug.Log($"Registered {typeof(TService).FullName} as {(isSingleton ? "singleton" : "transient")}");
        }

        public void RegisterFromInstance<TService>(TService instance)
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => instance,
                IsSingleton = true,
                Instance = instance
            };
            Debug.Log($"Registered instance of {typeof(TService).FullName}");
        }
        
        

        public void RegisterFactory<TService>(Func<object> factory)
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = factory ,
                IsSingleton = false,
                Instance = null
            };
            Debug.Log($"Registered instance of {typeof(TService).FullName}");
        }
        
        public void RegisterFromScene<TService>() where TService : Object
        {
            var instance = FindObjectOfType<TService>();
            if (instance == null)
            {
                throw new Exception($"Instance of type {typeof(TService).FullName} not found in scene");
            }
            
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => instance,
                IsSingleton = true,
                Instance = instance
            };
            Debug.Log($"Registered instance of {typeof(TService).FullName}");
        }
        
        public void RegisterMonoBehavior<TService>(TService prefab, bool isSingleton) where TService : MonoBehaviour
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(prefab), "Prefab cannot be null");
            }
            
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => GameObject.Instantiate(prefab),
                IsSingleton = isSingleton,
                Instance = isSingleton ? prefab : null
            };
        }
        
        protected object CreateInstance<T>()
        {
            
            var constructors = typeof(T).GetConstructors();
            
            
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var parameterInstances = parameters.Select(p =>_container.Resolve(p.ParameterType)).ToArray();

                return constructor.Invoke(parameterInstances);
            }

            return Activator.CreateInstance<T>();
            throw new Exception($"No suitable constructor found for type {typeof(T).FullName}");
        }


        
    }
}