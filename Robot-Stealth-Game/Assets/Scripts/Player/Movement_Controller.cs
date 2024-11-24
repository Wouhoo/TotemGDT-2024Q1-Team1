using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] public float defaultMaxSpeed = 10f;
    [SerializeField] public float movementAcceleration = 10f;
    public float speed;
    public float acceleration;

    void Start()
    {
        ResetMaxSpeed();
        rb.maxLinearVelocity = defaultMaxSpeed;
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        // apply walking acceleration
        rb.AddForce(direction * acceleration, ForceMode.Acceleration);
        speed = rb.velocity.magnitude;
    }

    public void SetMovement(float friction, float newMaxSpeed)
    {
        rb.maxLinearVelocity = newMaxSpeed;
        acceleration = movementAcceleration * friction + 10f;
    }

    public void ResetMaxSpeed()
    {
        SetMovement(0.5f, defaultMaxSpeed);
    }
}
