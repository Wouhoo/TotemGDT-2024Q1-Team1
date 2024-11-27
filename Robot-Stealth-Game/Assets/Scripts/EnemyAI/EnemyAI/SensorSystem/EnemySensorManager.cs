using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySensorManager : MonoBehaviour
{
    public bool bodyAttack = true; // if true, it will attack any part of the player prefering the body, else it will only attack arms.
    public bool canInfer = false; // if true, then if the target set is non empty, it knows where the player is.

    [HideInInspector] public Transform targetTransform = null; // this is for pursuing
    [HideInInspector] public Vector3 targetVector; // this is for investigating
    [HideInInspector] public bool targetVisited = true;
    public List<VisionSensor> visionSensors;


    private void Start()
    {
        // Register with sensor manager
        SensorManager.Instance.enemyManagers.Add(this);
    }

    public void VisionUpdate()
    {
        HashSet<GameObject> targets = new HashSet<GameObject>();
        foreach (VisionSensor sensor in visionSensors)
        {
            if (sensor != null)
                targets.UnionWith(sensor.targets);
        }
        TransformUpdate(targets);
    }

    public void TransformUpdate(HashSet<GameObject> targets)
    {
        // Then check if we have any targets
        if (targets.Count == 0)
        {
            if (targetTransform != null)
                VectorUpdate(targetTransform.position); // Save to last known position
            targetTransform = null;
            return;
        }
        GameObject playerObject = GameObject.FindWithTag("Player"); // (find with tag is bad!)
        if (canInfer || (bodyAttack && targets.Contains(playerObject)))
        {
            targetTransform = playerObject.transform;
            return;
        }
        // Otherwise we target the nearest
        TargetNearest(targets);
    }

    public void VectorUpdate(Vector3 newTarget)
    {
        // filter by newest
        targetVector = newTarget;
        targetVisited = false;
        return;
    }

    private void TargetNearest(HashSet<GameObject> targets)
    {
        float minDist = Mathf.Infinity;
        GameObject minTarget = null;
        Vector3 currentPos = transform.position;
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(currentPos, target.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                minTarget = target;
            }
        }
        targetTransform = minTarget.transform;
    }

    private void OnDestroy()
    {
        SensorManager.Instance.enemyManagers.Remove(this);
    }
}