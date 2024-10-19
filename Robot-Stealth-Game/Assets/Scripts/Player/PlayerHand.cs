using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHand : MonoBehaviour
{
    private Transform playerTransform;
    public float handSpeed = 5f;
    public float armLength = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // Check if hand is within max distance; if not we do nothing and let the spring take over
        if (Vector3.Distance(transform.position, playerTransform.position) > armLength)
            return;

        Vector3 direction = MouseWorldPosition.GetMouseWorldPosition() - transform.position;
        // we offset to the hand height
        direction.y = transform.position.y;

        // Check if the object is very close to the target
        if (direction.magnitude < 0.1f)
            return;

        // Only move if the object is far enough from the target
        //rb.MovePosition(transform.position + direction.normalized * handSpeed * Time.deltaTime);
        rb.AddForce(direction.normalized * handSpeed);
    }
}
