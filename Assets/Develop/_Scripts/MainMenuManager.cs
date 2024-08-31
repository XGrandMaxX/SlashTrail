using System;
using DI;
using UnityEngine;

namespace Develop._Scripts
{
   public class MainMenuManager : MonoBehaviour
   {
      [Inject] private PlayerInput _input;

      private void OnEnable()
      {
         _input.Input.Enable();
      }

      private void Awake()
      {
         _input = new PlayerInput();
         _input.Input.Jump.performed += c =>
         {
            print("jump");
         };
      }

      private void Start()
      {
         
      }

      private void Update()
      {
         print(_input.Input.MouseAxis.ReadValue<Vector2>());
      }

      private void OnDisable()
      {
         _input.Input.Disable();
      }
   }
}
