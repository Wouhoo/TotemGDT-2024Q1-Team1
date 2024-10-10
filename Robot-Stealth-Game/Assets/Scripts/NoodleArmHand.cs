using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleArmHand : MonoBehaviour
{
    public float handSpeed = 5f;
    public float maxArmLength = 10f;
    public Transform handTransform;
    private Vector3 targetPosition;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = MouseWorldPosition.GetMouseWorldPosition();
        //TODO: ensure the direction is on the ground
        //this is a bit of a hack to make this be on the ground for now
        targetPosition.y = 0.5f;

        // Ensure a valid position was hit
        if ((targetPosition != Vector3.zero) && (transform.position != targetPosition))
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0.5f;

            // Check if the object is very close to the target
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Only move if the object is far enough from the target
                // Move the object towards the mouse world position using Rigidbody.MovePosition
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, handSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);

                // After moving, rotate the object to face the direction it moved towards
                Vector3 moveDirection = newPosition - transform.position;
                if (moveDirection != Vector3.zero) // Ensure there's movement to avoid NaN rotations
                {
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
            }
            else
            {
                //prevents physics engine glitchyness
                rb.velocity = Vector3.zero;
            }
        }
    }
}
