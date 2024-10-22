using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmTargetNode : MonoBehaviour
{
    public KeyCode noodleKey = KeyCode.Space;

    public float defaultTargetYOffset;
    private float targetYOffset;

    void Start()
    {
        targetYOffset = defaultTargetYOffset;
    }

    void FixedUpdate()
    {
        Vector3 target = MouseWorldPosition.GetMouseWorldPosition();
        // We offset to the hand height
        target.y = targetYOffset;
        transform.position = target;
    }

    // Set the target to the ground when clicked
    void OnMouseDown()
    {
        targetYOffset = 0f;
    }
    void OnMouseUp()
    {
        targetYOffset = defaultTargetYOffset;
    }
}
