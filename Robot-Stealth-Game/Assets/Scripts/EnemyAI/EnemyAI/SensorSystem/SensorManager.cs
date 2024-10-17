using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// This script manages all sensors in one place for enemies to reference
/// Each sensor type should have its own HashSet, and this should be a set of game objects (not sensor classes!)
/// There should also be registery and deregistry functions for each sensor type.
/// (note: in the future the registry functions might be folded into just one for simplicity (?))
/// 
/// </summary>

public class SensorManager : MonoBehaviour
{
    public static SensorManager Instance { get; private set; } = null;

    public HashSet<GameObject> targets = new HashSet<GameObject>();
    public HashSet<EnemySensorManager> enemyManagers = new HashSet<EnemySensorManager>();
    public HashSet<VisionSensor> visionSensors = new HashSet<VisionSensor>();
    public HashSet<HearingSensor> hearingSensors = new HashSet<HearingSensor>();
    // public HashSet<GameObject> proximitySensors = new HashSet<GameObject>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple SensorManagers found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void VisionSensorUpdate()
    {
        HashSet<GameObject> oldTargets = new HashSet<GameObject>(targets);
        targets.Clear();
        foreach (VisionSensor sensor in visionSensors)
        {
            if (sensor != null)
                targets.UnionWith(sensor.targets);
        }
        if (targets.SetEquals(oldTargets))
            return;
        foreach (EnemySensorManager enemy in enemyManagers)
            enemy.TransformUpdate(targets);
    }
}