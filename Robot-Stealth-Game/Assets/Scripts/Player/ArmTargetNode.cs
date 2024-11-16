using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using UnityEngine.InputSystem;

public class ArmTargetNode : MonoBehaviour
{
    public KeyCode deployArmKey = KeyCode.Mouse0;
    // InputAction interactAction;
    public float shoulderHeight = 1f;
    public float groundHeight = 0.2f;
    private float targetYOffset;

    private void Start()
    {
        // interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void OnEnable()
    {
        targetYOffset = shoulderHeight;
    }

    void Update()
    {
        Vector3 target = MouseWorldPosition.GetMouseWorldPosition();
        // We offset to the hand height
        target.y = targetYOffset;
        transform.position = target;

        if (Input.GetKeyDown(deployArmKey))
        {
            // if by some special object do this stuff ...
            // else we gonna make some noise!!!!
            targetYOffset = groundHeight;
            MakeNoise();
            return;
        }
        if (Input.GetKeyUp(deployArmKey))
        {
            targetYOffset = shoulderHeight;
            return;
        }
    }

    private void MakeNoise()
    {
        // TODO: play some sound
        Vector3 pos = transform.position;
        foreach (HearingSensor sensor in SensorManager.Instance.hearingSensors)
            sensor.SensorUpdate(pos);
    }

}
