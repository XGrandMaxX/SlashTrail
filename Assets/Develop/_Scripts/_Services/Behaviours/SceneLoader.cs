using System;
using Cysharp.Threading.Tasks;
using Develop._Scripts._Services.Abstractions;
using UnityEngine.SceneManagement;

namespace Develop._Scripts._Services.Behaviours
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadScene(string path)
        {
            await SceneManager.LoadSceneAsync(path);
        }
    }
}
