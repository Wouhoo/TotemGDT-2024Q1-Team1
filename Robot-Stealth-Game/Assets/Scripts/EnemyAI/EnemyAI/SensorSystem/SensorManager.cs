using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SensorManager : MonoBehaviour
{
    public static SensorManager Instance { get; private set; } = null;
    // public HashSet<GameObject> [XXX]Sensors = new HashSet<GameObject>();
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