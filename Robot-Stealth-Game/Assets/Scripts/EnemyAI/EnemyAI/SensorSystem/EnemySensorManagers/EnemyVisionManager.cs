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

/// <summary>
/// 
/// An instance of this script is initialized by an enemy.
/// This script manages which vision sensors it can "see" trhough.
/// The sensors must be provided by the enemy.
/// This script also manages the logic of how to process its visual information.
/// 
/// </summary>

public class EnemyVisionManager
{
    public Transform targetTransform; // The targets transform (null if not visable)
    public Vector3 targetVector; // The last position of the last seen target before it left our vision

    public HashSet<VisionSensor> visionSensors = new HashSet<VisionSensor>();
    public HashSet<GameObject> visionTargets = new HashSet<GameObject>();

    private readonly Transform location;
    private readonly int canTarget; // ie. can attack body
    private readonly int preference; // ie. prefer to attack nearest

    public EnemyVisionManager(Transform location, int canTarget = 2, int preference = 0)
    {
        this.location = location;
        this.canTarget = canTarget;
        // can target: 0 (body only) , 1 (arms only) , 2 (both arms and body) 
        this.preference = preference;
        // prefers to target: 0 (body) , 1 (nearest arm) , 2 (nearest object)
    }

    // The following function will be called by the enemy class, telling us which vision sensors we should be using
    public void ListenersRefresh(HashSet<VisionSensor> newSensors)
    {
        if (newSensors.SetEquals(visionSensors)) // Don't run if not needed
            return;

        // We now subscribe to the new sensors
        HashSet<VisionSensor> newListeners = new HashSet<VisionSensor>(newSensors);
        newListeners.ExceptWith(visionSensors);
        foreach (VisionSensor newSensor in newListeners)
        {
            if (newSensor != null)
            {
                visionSensors.Add(newSensor); // Add before registering!
                newSensor.RegisterEnemy(this);
            }
        }

        // We now unsubscribe from the old sensors
        HashSet<VisionSensor> oldListeners = new HashSet<VisionSensor>(visionSensors);
        oldListeners.ExceptWith(newSensors);
        foreach (VisionSensor oldSensor in oldListeners)
        {
            if (oldSensor != null)
                oldSensor.DeregisterEnemy(this); // Deregister before removing!
            visionSensors.Remove(oldSensor);
        }
    }

    // The following function is called when what is in our vision changes
    public void VisionUpdate()
    {
        HashSet<GameObject> oldTargets = new HashSet<GameObject>(visionTargets);
        // We first get the current visable targets
        visionTargets.Clear();
        foreach (VisionSensor sensor in visionSensors)
        {
            if (sensor != null) // Saftey
                visionTargets.UnionWith(sensor.targets);
            else
                visionSensors.Remove(sensor);
        }

        // First check if any changes
        if (oldTargets == visionTargets)
            return; // No changes

        // Then check if we have any targets
        if (visionTargets.Count == 0)
        {
            NullifyTargets();
            return;
        }

        GameObject playerObject = GameObject.FindWithTag("Player"); // (find with tag is bad!)
        // ALERT : The following is the (fastest?) decision tree to handel targeting logic
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

    // The following function activates when our line of sight is cleared; so the reansform is emptied out, and the last seen position vector is updated
    private void NullifyTargets()
    {
        if (targetTransform != null)
            targetVector = targetTransform.position; // Save to last known position
        targetTransform = null;
        // else do nothing
    }

    // The following function gets the nearest target out of the visable targets; and updates the target transform
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
        // Unsubscribe from the sensors to prevent memory leaks
        foreach (VisionSensor sensor in visionSensors)
        {
            if (sensor != null)
                sensor.DeregisterEnemy(this);
        }
    }
}

