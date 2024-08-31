using System;
using Cysharp.Threading.Tasks;
using Develop._Scripts._Services.Abstractions;
using DI;
using UnityEngine;

namespace Develop._Scripts.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;

        private async void Awake()
        {
            await _sceneLoader.LoadScene("MainMenu");
        }

    }
}
