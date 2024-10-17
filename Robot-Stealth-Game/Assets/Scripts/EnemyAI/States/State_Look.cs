using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Look : IState
{
    private readonly float lookAngle; // in degrees
    private readonly float rotationSpeed;
    private bool isDone = false;
    private Quaternion target0;
    private Quaternion target1;
    private Quaternion target2;

    private MonoBehaviour parent; // Reference to start coroutine
    private Coroutine rotationCoroutine;

    public State_Look(MonoBehaviour parent, float lookAngle, float rotationSpeed)
    {
        this.parent = parent; // Store reference to the MonoBehaviour
        this.lookAngle = lookAngle;
        this.rotationSpeed = rotationSpeed / lookAngle;
    }

    public void OnEnter()
    {
        Debug.Log("Look : Enter");
        target0 = parent.transform.rotation;
        target1 = target0 * Quaternion.AngleAxis(lookAngle, Vector3.up);
        target2 = target0 * Quaternion.AngleAxis(-lookAngle, Vector3.up);

        isDone = false;
        rotationCoroutine = parent.StartCoroutine(SmoothLookRoutine());
    }

    public void Tick()
    {
    }

    private IEnumerator SmoothLookRoutine()
    {
        float progress = 0f;

        // Rotate towards the first target (look left)
        while (progress < 1f)
        {
            progress += Time.deltaTime * rotationSpeed;
            parent.transform.rotation = Quaternion.Slerp(target0, target1, progress);
            yield return null; // Wait until the next frame
        }

        progress = 0f;

        // Rotate back towards the middle
        while (progress < 1f)
        {
            progress += Time.deltaTime * rotationSpeed;
            parent.transform.rotation = Quaternion.Slerp(target1, target0, progress);
            yield return null; // Wait until the next frame
        }

        progress = 0f;

        // Rotate towards the second target (look right)
        while (progress < 1f)
        {
            progress += Time.deltaTime * rotationSpeed;
            parent.transform.rotation = Quaternion.Slerp(target0, target2, progress);
            yield return null; // Wait until the next frame
        }

        // Rotation is complete
        isDone = true;
        parent.StopCoroutine(rotationCoroutine);
    }

    public void OnExit()
    {
        Debug.Log("Look : Exit");
        isDone = false; // we always default to is done being false
        parent.StopCoroutine(rotationCoroutine);
    }

    public bool IsDone()
    {
        return isDone;
    }
}