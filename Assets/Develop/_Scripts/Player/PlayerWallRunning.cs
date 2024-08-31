using System;
using System.Collections;
using System.Collections.Generic;
using DI;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallRunning : MonoBehaviour
{
    [Inject] private PlayerInput _input;

    private void OnEnable()
    {
        _input.Input.Enable();
    }

    private void OnDisable()
    {
        _input.Input.Disable();
    }

    [Header("Variables")] 
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    public float wallJumpUpForce;
    public float wallJumpSideForce;

    private bool exitingWall = false;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Input")] 
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")] 
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")] 
    public Transform orientation;

    private PlayerController pc;
    private Rigidbody rb;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pc.wallruning)
            HandleWallRun();
    }

    private void CheckWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit,wallCheckDistance, wallLayer);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit,wallCheckDistance, wallLayer);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayer);
    }

    private void StateMachine()
    {
        horizontalInput = _input.Input.InputAxis.ReadValue<Vector2>().x;
        verticalInput = _input.Input.InputAxis.ReadValue<Vector2>().y;

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if(!pc.wallruning)
                StartWallRun();
            
            if(Input.GetKeyDown(KeyCode.Space))
                WallJump();
        }
        else if (exitingWall)
        {
            if(pc.wallruning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        else
            StopWallRun();
        
    }

    private void StartWallRun()
    {
        pc.wallruning = true;
    }

    private void StopWallRun()
    {
        pc.wallruning = false;
    }

    private void HandleWallRun()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        
        rb.AddForce(wallForward * wallRunForce,ForceMode.Force);
        
        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100,ForceMode.Force);

        
        
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(forceToApply,ForceMode.Impulse);
    }
}
