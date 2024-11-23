using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement_Controller : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset = new Vector3(0,7,0);
    public Vector3 rotationOffset = new Vector3(100,0,0);

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + target.rotation * locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion desiredrotation = target.rotation * Quaternion.Euler(rotationOffset);
        Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
        transform.rotation = smoothedrotation;
    }
}
