using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Im really tired, i'll add proper comments to this file some other time
/// plus it'll probably still change a bunch, so why bother
/// 
/// </summary>

public class VisionSensor : MonoBehaviour
{
    public float tickRate = 0.2f;
    [HideInInspector] public HashSet<GameObject> targets = new HashSet<GameObject>();
    [HideInInspector] public HashSet<EnemySensorManager> enemyManagers = new HashSet<EnemySensorManager>();

    public float visionRadius = 30f;
    public float visionAngle = 60f;
    private float cosVisionConeAngle;
    private LayerMask playerMask;
    private LayerMask layerMask;

    private void Awake()
    {
        playerMask = LayerMask.GetMask("Player");
        layerMask = LayerMask.GetMask("Player", "Obstacle");
        cosVisionConeAngle = Mathf.Cos(visionAngle * Mathf.Deg2Rad);
    }

    private void Start()
    {
        // Register this sensor with the sensor manager
        SensorManager.Instance.visionSensors.Add(this);
        InvokeRepeating("SensorUpdate", tickRate, tickRate);
    }

    public void SensorUpdate()
    {
        // We get the visable targets
        HashSet<GameObject> detectedTargets = new HashSet<GameObject>();
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, visionRadius, playerMask);
        foreach (var target in targetsInRange)
        {
            if (CheckTarget(target))
                detectedTargets.Add(target.gameObject);
        }
        // If the observable targets has changed, we tell the sensor manager
        if (!detectedTargets.SetEquals(targets))
        {
            targets = detectedTargets;
            SensorManager.Instance.VisionSensorUpdate();
        }
    }

    private bool CheckTarget(Collider target)
    {
        // check if in vision cone
        Vector3 vectorToTarget = (target.transform.position - transform.position).normalized;
        if (Vector3.Dot(vectorToTarget, transform.forward) < cosVisionConeAngle)
            return false;

        // raycast to target passes?
        RaycastHit hitResult;
        if (Physics.Raycast(transform.position, vectorToTarget, out hitResult, visionRadius, layerMask))
        {
            if (hitResult.collider == target)
                return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        SensorManager.Instance.visionSensors.Remove(this);
        CancelInvoke();
    }
}