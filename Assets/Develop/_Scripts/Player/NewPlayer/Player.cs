using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;

    [SerializeField] private PlayerCamera _playerCamera;
    [Space] 
    [SerializeField] private CameraSpring _cameraSpring;
    [SerializeField] private CameraLean _cameraLean;

    private PlayerInputActions _inputActions;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        
        _playerCharacter.Initialize();
        _playerCamera.Initialize(_playerCharacter.GetCameraTarget());
        
        _cameraSpring.Initialize();
        _cameraLean.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        var input = _inputActions.Gameplay;
        var deltaTime = Time.deltaTime;

        var cameraInput = new CameraInput() { Look = input.Look.ReadValue<Vector2>() };
        _playerCamera.UpdateRotation(cameraInput);

        var characterInput = new CharacterInput()
        {
            Rotation = _playerCamera.transform.rotation,
            Move     = input.Move.ReadValue<Vector2>(),
            Jump     = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Crouch = input.Crouch.WasPressedThisFrame()
            ? CrouchInput.Toggle :
            CrouchInput.None
            
        };
        
        _playerCharacter.UpdateInput(characterInput);
        _playerCharacter.UpdateBody(deltaTime);

#if UNITY_EDITOR
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            var ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit))
            {
                Teleport(hit.point);
            }
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            _playerCharacter.Throw(-_playerCamera.transform.forward,2);
        }
        
#endif
        
    }

    private void LateUpdate()
    {
        var cameraTarget = _playerCharacter.GetCameraTarget();
        var state = _playerCharacter.GetState();
        
        _playerCamera.UpdatePosition(cameraTarget);
        _cameraSpring.UpdateSpring(Time.deltaTime,cameraTarget.up);
        _cameraLean.UpdateLean(Time.deltaTime,state.Acceleration,cameraTarget.up);
    }

    private void OnDestroy()
    {
        _inputActions.Dispose();
    }

    public void Teleport(Vector3 pos)
    {
        _playerCharacter.SetPosition(pos);
    }
}
