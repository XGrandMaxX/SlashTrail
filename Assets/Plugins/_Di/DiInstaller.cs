using DI;
using UnityEngine;

namespace _Di
{
    public class DiInstaller : MonoBehaviour
    {
        private DiContainer _container;
        private void Awake()
        {
            //Initialize();
        }
        
        public void Initialize()
        {
            print(99);
            var objects = FindObjectsOfType<DiInstaller>();
            if(objects.Length > 1)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        
            _container = DiContainer.Instance;
        
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (s, c) =>
            {
                ControlProjectInjection();
            };
        }

        private void ControlProjectInjection()
        {
            _container.ClearSceneDependencies();
        
            var pContext = FindObjectOfType<ProjectContext>();
            if (pContext != null)
            {
                pContext.RegisterDependencies();
                _container.SetProjectDependencies(pContext._registrations);
            }
        
            var sContext = FindObjectOfType<SceneContext>();
            if (sContext != null)
            {
                print("1231231");
                sContext.RegisterDependencies();
                _container.SetSceneDependencies(sContext._registrations);
            }
        
            _container.Initialize();
        }
    
    }
}
