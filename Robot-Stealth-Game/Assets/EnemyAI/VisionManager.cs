using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

public class VisionManager
{
    public Transform targetTransform;
    public Vector3 targetVector;

    public HashSet<VisionSensor> visionSensors = new HashSet<VisionSensor>();
    public HashSet<GameObject> visionTargets = new HashSet<GameObject>();

    private readonly Transform location;
    private readonly int canTarget; // ie. can attack body
    private readonly int preference; // ie. prefer to attack nearest

    public VisionManager(Transform location, int canTarget = 2, int preference = 0) // 0: body, 1: arms, 2: (can:both) (pref:near)
    {
        this.location = location;
        this.canTarget = canTarget;
        this.preference = preference;
    }

    public void VisionListenersRefresh(HashSet<VisionSensor> newSensors)
    {
        if (newSensors.SetEquals(visionSensors)) // Don't run if not needed
            return;

        // We now subscribe to the new sensors
        HashSet<VisionSensor> newListeners = new HashSet<VisionSensor>(newSensors);
        newListeners.ExceptWith(visionSensors);
        foreach (VisionSensor newSensor in newListeners)
        {
            visionSensors.Add(newSensor); // Already before to avoid OnDestroy missing listeners halfway through
            newSensor.OnVisionChange += VisionUpdate;
        }

        // We now unsubscribe from the old sensors
        HashSet<VisionSensor> oldListeners = new HashSet<VisionSensor>(visionSensors);
        oldListeners.ExceptWith(newSensors);
        foreach (VisionSensor oldSensor in oldListeners)
        {
            oldSensor.OnVisionChange -= VisionUpdate;
        }

        // We now update our set of listened to sensors (AFTER UNSUBSCRIBING)
        visionSensors = newSensors;
        Debug.Log(visionSensors);
    }

    private void VisionUpdate()
    {
        Debug.Log("VisionUpdate");
        HashSet<GameObject> oldTargets = new HashSet<GameObject>(visionTargets);
        // Get the current visable targets
        visionTargets.Clear();
        foreach (VisionSensor sensor in visionSensors)
        {
            if (sensor != null) // Saftey
            {
                visionTargets.UnionWith(sensor.targets);
            }
        }

        if (oldTargets == visionTargets)
            return; // No changes

        if (visionTargets.Count == 0)
        {
            NullifyTargets();
            return;
        }

        GameObject playerObject = GameObject.FindWithTag("Player"); // (find with tag is bad!)
        // The following is the fastest decision tree to targeting
        if (canTarget == 0) // Can only target body
        {
            if (visionTargets.Contains(playerObject))
            {
                targetTransform = playerObject.transform; // Target the body
                return;
            }
            // else, has no targets
            NullifyTargets();
            return;
        }
        if (canTarget == 1) // Can only target arms
        {
            if (visionTargets.Count != 1) // More than one object 
            {
                if (visionTargets.Contains(playerObject) == true) // If body in set
                    visionTargets.Remove(playerObject);
                TargetNearest();
                return;
            }
            // else, only 1 object
            if (visionTargets.Contains(playerObject) == true) // If body in set
            {
                NullifyTargets();
                return;
            }
            // else, only object must be arm
            TargetNearest();
            return;
        }
        // else canTarget = 2 (both)
        if (preference == 0) // prefer body
        {
            if (visionTargets.Contains(playerObject))
            {
                targetTransform = playerObject.transform; // Target the body
                return;
            }
            // else, target nearest
            TargetNearest();
            return;
        }
        if (preference == 2) // prefer nearest
        {
            TargetNearest();
            return;
        }
        // else, preference == 1 : prefer arms
        if (visionTargets.Count != 1) // More than one object 
        {
            if (visionTargets.Contains(playerObject) == true) // If body in set
                visionTargets.Remove(playerObject);
            TargetNearest();
            return;
        }
        // else, only 1 object
        targetTransform = visionTargets.Single().transform;
        return;
    }

    private void NullifyTargets()
    {
        if (targetTransform != null)
            targetVector = targetTransform.position; // Save to last known position
        targetTransform = null;
        // else do nothing
    }

    private void TargetNearest()
    {
        float minDist = Mathf.Infinity;
        GameObject minTarget = null;
        Vector3 currentPos = location.position;
        foreach (GameObject target in visionTargets)
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

    void OnDestroy()
    {
        // Unsubscribe from the sensors' events to prevent memory leaks
        foreach (VisionSensor sensor in visionSensors)
        {
            sensor.OnVisionChange -= VisionUpdate;
        }
    }
}

