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
    // public HashSet<GameObject> HearingSensors = new HashSet<GameObject>();
    // public HashSet<GameObject> MovmentSensors = new HashSet<GameObject>();
    // public HashSet<GameObject> ProximitySensors = new HashSet<GameObject>();
    public HashSet<GameObject> VisionSensors = new HashSet<GameObject>();

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

    public void RegisterVisionSensor(GameObject sensor)
    {
        VisionSensors.Add(sensor);
    }

    public void DeregisterVisionSensor(GameObject sensor)
    {
        VisionSensors.Remove(sensor);
    }
}