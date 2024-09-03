using TMPro;
using UnityEngine;

namespace Develop._Scripts.Bootstrap
{
    public class FPSController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsDisplay;
        private float _deltaTime = 0.0f;

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

            float fps = 1.0f / _deltaTime;

            _fpsDisplay.text = $"{fps:0.} fps";
        }
    }
}
