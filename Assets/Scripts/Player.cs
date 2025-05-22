using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;
    public event System.Action OnPlayerSeen;
    public event System.Action OnPlayerPast;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    private float smoothInputMagnitude;
    private float smoothMoveVelocity;
    private float angle;
    private Vector3 velocity;
    private Rigidbody rb;

    private bool disable = false;

    
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardSpottedPlayer += Disable;
        
    }

    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        if(!disable)
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime * inputMagnitude);
        
        velocity = transform.forward * speed * smoothInputMagnitude;
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.AddCoin();
            Destroy(other.gameObject);
            
        }

        if (other.CompareTag("End"))
        {
            if(OnPlayerPast != null)
                OnPlayerPast();
            disable = true;
        }
    }

    private void Disable()
    {
        disable = true;
        if(OnPlayerSeen != null)
            OnPlayerSeen();
    }
    private void OnDestroy()
    {
        Guard.OnGuardSpottedPlayer -= Disable;
    }
}
