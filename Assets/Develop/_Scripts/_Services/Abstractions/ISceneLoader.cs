using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Develop._Scripts._Services.Abstractions
{
    public interface ISceneLoader
    {
        public UniTask LoadScene(string path);

    }
}
