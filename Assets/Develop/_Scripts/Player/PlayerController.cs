using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DI;
using UnityEngine;

public enum MoveState {walk,run,crouch,air,wallrun}
public class PlayerController : MonoBehaviour
{
    [Inject] private PlayerInput _input;

    [Header("Movement")] 
    private float moveSpeed;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float wallrunSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool isReadyToJump = true;
   

    [Header("Ground Check")] 
    public float playerHeight;

    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Other")] 
    private MoveState state;

    public bool wallruning;

    public float crouchScale = 0.5f;
    public float defaultScale = 1f;
        
    public Transform orientation;

    [Header("Slope")] public float slopeAngle = 40;
    private RaycastHit slopeHit;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    
  
    

    private void OnEnable()
    {
        _input.Input.Enable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //_input.Input.Jump.performed += c => HandleJump();
    }

    private void HandleInput()
    {
        horizontalInput = _input.Input.InputAxis.ReadValue<Vector2>().x;
        verticalInput = _input.Input.InputAxis.ReadValue<Vector2>().y;
    }

    private void StateHandler()
    {
        if (wallruning)
        {
            state = MoveState.wallrun;
            moveSpeed = wallrunSpeed;
        }
        if (isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = MoveState.run;
            moveSpeed = runSpeed;
        }
        else if (isGrounded)
        {
            state = MoveState.walk;
            moveSpeed = walkSpeed;
        }
        else
            state = MoveState.air;
        
        /*if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MoveState.crouch;
            moveSpeed = crouchSpeed;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale =
                new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            rb.AddForce(Vector3.down * .5f,ForceMode.Impulse);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale =
                new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);
            rb.AddForce(Vector3.down * .5f,ForceMode.Impulse);
        }*/
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isSlope())
        {
            rb.AddForce(GetSlope() * moveSpeed * 20f,ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f,ForceMode.Force);
            }
        }
        
        else if(isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f,ForceMode.Force);
        else if(!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier,ForceMode.Force);

        rb.useGravity = !isSlope();
    }

    private void Update()
    {
        HandleInput();
        SpeedControl();
        StateHandler();
        
        print(rb.velocity.magnitude);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        
        if(_input.Input.Jump.ReadValue<float>() > 0)
            HandleJump();
    }

    private void OnDisable()
    {
        _input.Input.Disable();
    }

    private void SpeedControl()
    {
        
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y,limitedVel.z);
        }
        
    }

    private void HandleJump()
    {
        if (!isGrounded || !isReadyToJump)
            return;
        isReadyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
        
        /*Vector3 targetVelocity = moveDirection * moveSpeed;
        Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);*/
        
        Invoke(nameof(ResetJump),jumpCooldown);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private bool isSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < slopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlope()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
