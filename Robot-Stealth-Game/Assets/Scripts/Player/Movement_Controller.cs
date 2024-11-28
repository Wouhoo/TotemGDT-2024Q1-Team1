using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] public float defaultMaxSpeed = 10f;
    [SerializeField] public float defaultfriction = 0.6f;
    [SerializeField] public float defaultMovementAcceleration = 10f;
    public float speed;
    public float movementAcceleration;
    public float stoppingAcceleration;

    void Start()
    {
        ResetMaxSpeed();
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (direction.magnitude < 0.2f) // not walking
        {
            if (rb.velocity.magnitude > 1f) // apply stopping force
            {
                Vector3 stoppingDirection = new Vector3(-rb.velocity.x, 0f, -rb.velocity.z);
                rb.AddForce(stoppingDirection.normalized * stoppingAcceleration, ForceMode.Acceleration);
            }
        }
        else // apply walking acceleration
            rb.AddForce(direction.normalized * movementAcceleration, ForceMode.Acceleration);

        speed = rb.velocity.magnitude;
    }

    public void SetMovement(float friction, float newMaxSpeed)
    {
        rb.maxLinearVelocity = newMaxSpeed;

        movementAcceleration = defaultMovementAcceleration * friction + 10 * (defaultfriction - 0.1f);

        if (friction >= defaultfriction - 0.1f) // sufficient friction to stop ourselves
            stoppingAcceleration = movementAcceleration;
        else
            stoppingAcceleration = 0f;
    }

    public void ResetMaxSpeed()
    {
        SetMovement(defaultfriction, defaultMaxSpeed);
    }
}
