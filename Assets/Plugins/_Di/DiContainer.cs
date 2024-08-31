using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using _Di;
using UnityEngine;

namespace DI
{
    public class DiContainer 
    {
        private static Dictionary<Type, Registration> _projectRegistrations = new();
        private static Dictionary<Type, Registration> _sceneRegistrations = new();
        private static readonly HashSet<Type> _resolves = new();

        public static DiContainer Instance
        {
            get
            {
                if (instance == null)
                    instance = new DiContainer();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        private static DiContainer instance;
        
        private DiContainer()
        {
            
        }

        public void SetProjectDependencies(Dictionary<Type, Registration> projectRegistrations)
        {
            _projectRegistrations = projectRegistrations;
            _projectRegistrations[typeof(DiContainer)] = new Registration
            {
                IsSingleton = true,
                Instance = Instance
            };
        }
        
        public void SetSceneDependencies(Dictionary<Type, Registration> sceneRegistrations)
        {
            _sceneRegistrations = sceneRegistrations;
        }

        public void ClearSceneDependencies()
        {
            _sceneRegistrations = new();
        }

        public object Resolve(Type parameter)
        {
            try
            {
                if (_resolves.Contains(parameter))
                {
                    throw new Exception("Cycled dependency detected");
                }

                _resolves.Add(parameter);
                
                if (_sceneRegistrations.TryGetValue(parameter, out var reg))
                {
                    if (reg.IsSingleton)
                    {
                        if (reg.Instance == null && reg.Factory != null)
                        {
                            reg.Instance = reg.Factory();
                            InjectDependenciesInto(reg.Instance);
                        }
                        return reg.Instance;
                    }

                    var instance = reg.Factory();
                    InjectDependenciesInto(instance);
                    return instance;
                }

                if (_projectRegistrations.TryGetValue(parameter, out var registration))
                {
                    if (registration.IsSingleton)
                    {
                        if (registration.Instance == null && registration.Factory != null)
                        {
                            registration.Instance = registration.Factory();
                            InjectDependenciesInto(registration.Instance);
                        }
                        return registration.Instance;
                    }

                    var instance = registration.Factory();
                    InjectDependenciesInto(instance);
                    return instance;
                }
                else
                {
                    throw new Exception($"Dependency with type {parameter.FullName} was not registered");
                }
            }
            finally
            {
                _resolves.Remove(parameter);
            }
        }

        

        private void InjectDependencies()
        {
            var allMonoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();

            foreach (var monoBehaviour in allMonoBehaviours)
            {
                InjectDependenciesInto(monoBehaviour);
            }
        }

        private void InjectDependenciesInto(object obj)
        {
            InjectDependenciesIntoMethods(obj);
            InjectDependenciesIntoProperties(obj);
            InjectDependenciesIntoFields(obj);
        }
        
        private void InjectDependenciesIntoMethods(object obj)
        {
            var methods = obj.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(InjectAttribute), true).Any());

            foreach (var method in methods)
            {
                var parameters = method.GetParameters()
                    .Select(p => Resolve(p.ParameterType))
                    .ToArray();

                method.Invoke(obj, parameters);
            }
        }
        
        private void InjectDependenciesIntoFields(object obj)
        {
            var fields = obj.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttributes(typeof(InjectAttribute), true).Any());
            
            foreach (var field in fields)
            {
                var dependency = Resolve(field.FieldType);
                field.SetValue(obj, dependency);
            }
        }
        private void InjectDependenciesIntoProperties(object obj)
        {
            var properties = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttributes(typeof(InjectAttribute), true).Any());
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    var dependency = Resolve(property.PropertyType);
                    property.SetValue(obj, dependency);
                }
            }
           
        }

        public void Initialize()
        {
            InjectDependencies();
        }

        public static UnityEngine.Object Instantiate(UnityEngine.Object original)
        {
            var obj = GameObject.Instantiate(original);
            Instance.InjectDependenciesInto(obj);
            return obj;
        }
        
        public static UnityEngine.Object Instantiate(UnityEngine.Object original,Vector3 pos,Quaternion rotation)
        {
            var obj = GameObject.Instantiate(original,pos,rotation);
            Instance.InjectDependenciesInto(obj);
            return obj;
        }
        
        public static T Instantiate<T>(T original) where T : Component
        {
            var obj = GameObject.Instantiate(original);
            Instance.InjectDependenciesInto(obj);
            return obj;
        }
        
        public static T Instantiate<T>(string name) where T : Component
        {
            var obj = new GameObject(name).AddComponent<T>();
            Instance.InjectDependenciesInto(obj);
            return obj;
        }
        
        public static T Instantiate<T>(T original,Vector3 pos,Quaternion rotation) where T : Component
        {
            var obj = GameObject.Instantiate(original,pos,rotation);
            Instance.InjectDependenciesInto(obj);
            return obj;
        }
        
    }

   

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class InjectAttribute : Attribute
    {
    }
}
