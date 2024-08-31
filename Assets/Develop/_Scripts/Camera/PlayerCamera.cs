using System;
using DI;
using UnityEngine;

namespace Develop._Scripts.Camera
{
    public class PlayerCamera : MonoBehaviour
    {
        [Inject] private PlayerInput _input;

        private void OnEnable()
        {
            _input.Input.Enable();
        }

        private void Update()
        {
            var mouseX = _input.Input.MouseAxis.ReadValue<Vector2>().x * Time.deltaTime * sensX;
            var mouseY = _input.Input.MouseAxis.ReadValue<Vector2>().y * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90, 90);
            
            transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
            orientation.rotation = Quaternion.Euler(0,yRotation,0);
        }

        private void OnDisable()
        {
            _input.Input.Disable();
        }

        public float sensX;
        public float sensY;

        public Transform orientation;

        private float xRotation;
        private float yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
