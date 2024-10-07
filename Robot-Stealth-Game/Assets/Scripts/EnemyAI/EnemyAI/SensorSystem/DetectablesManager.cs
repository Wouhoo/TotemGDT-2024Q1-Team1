using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// DO NOT EDIT!
/// This script tracks all detectable objects; those being things sensors can see or touch.
/// Must be included in scene.
/// 
/// </summary>

public class DetectablesManager : MonoBehaviour
{
    public static DetectablesManager Instance { get; private set; } = null;
    public HashSet<GameObject> AllTargets { get; private set; } = new HashSet<GameObject>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple DetectablesManager found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterTarget(GameObject target)
    {
        AllTargets.Add(target);
    }

    public void DeregisterTarget(GameObject target)
    {
        AllTargets.Remove(target);
    }
}