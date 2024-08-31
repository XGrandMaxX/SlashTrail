using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DI;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Inject] private PlayerInput _input;
    
    [Header("References")] public Transform orientation;
    public Transform player;
    private Rigidbody rb;
    private PlayerController pc;

    [Header("Sliding")] public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale = .5f;
    private float startYScale = 1f;

    private KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
        
        
    }

    private void OnEnable()
    {
        _input.Input.Enable();
    }

    private void OnDestroy()
    {
        _input.Input.Disable();
    }

    private void Update()
    {
        horizontalInput = _input.Input.InputAxis.ReadValue<Vector2>().x;
        verticalInput = _input.Input.InputAxis.ReadValue<Vector2>().y;
        
        if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();
        
        if(Input.GetKeyUp(slideKey) && sliding)
            StopSlide();
        
        if(Input.GetKeyDown(KeyCode.Space))
            StopSlide();
    }

    private void FixedUpdate()
    {
        if(sliding)
            HandleSlide();
    }

    private void StartSlide()
    {
        sliding = true;

        player.localScale = new Vector3(player.localScale.x, slideYScale, player.localScale.z);
        rb.AddForce(Vector3.down * 5f,ForceMode.Impulse);
        slideTimer = maxSlideTime;
    }

    private void StopSlide()
    {
        sliding = false;

        player.localScale = new Vector3(player.localScale.x, startYScale, player.localScale.z);
    }

    private void HandleSlide()
    {
        Vector3 input = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        rb.AddForce(input.normalized * slideForce,ForceMode.Force);

        slideTimer -= Time.deltaTime;
        
        if(slideTimer <= 0)
            StopSlide();
    }
}
