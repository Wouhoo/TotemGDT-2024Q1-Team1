using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHand : MonoBehaviour
{
    public KeyCode noodleKey = KeyCode.Space;
    public float handForce = 5f;
    private Transform playerTransform;
    private Rigidbody rb;
    private bool armDeployed = false;
    private ConfigurableJoint joint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        joint = GetComponent<ConfigurableJoint>();
    }

    void FixedUpdate()
    {
        // Check if the arm is deployed
        if (!armDeployed)
        {
            return;
        }

        Vector3 direction = MouseWorldPosition.GetMouseWorldPosition(); // - transform.position;
        // we offset to the hand height
        direction.y = transform.position.y;

        // Check if the object is very close to the target
        //if (direction.magnitude < 0.1f)
        //    return;

        // Apply force in direction of hand
        // rb.AddForce(direction * handForce);
        transform.position = direction;
    }

    void Update()
    {
        if (Input.GetKeyDown(noodleKey))
        {
            if (armDeployed)
                joint.connectedBody = null;
            else
                joint.connectedBody = null;
            armDeployed = !armDeployed;
        }
    }
}
