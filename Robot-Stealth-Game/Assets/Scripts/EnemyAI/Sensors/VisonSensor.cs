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
    public float tickSpeed = 0.2f;
    [HideInInspector] public HashSet<GameObject> targets = new HashSet<GameObject>();
    [HideInInspector] public HashSet<EnemyVisionManager> enemyManagers = new HashSet<EnemyVisionManager>();
    public float visionRadius = 30f;
    public float visionAngle = 60f;
    public float cosVisionConeAngle { get; private set; } = 0f;

    [SerializeField] LayerMask visionLayer;
    [SerializeField] private LayerMask visionObstacleLayer;

    private float tickDeadline;
    private void Awake()
    {
        cosVisionConeAngle = Mathf.Cos(visionAngle * Mathf.Deg2Rad);
    }

    private void Update()
    {
        if (Time.time >= tickDeadline)
        {
            tickDeadline = Time.time + tickSpeed;
            HashSet<GameObject> newTargets = SensorUpdate();
            if (!newTargets.SetEquals(targets))
            {
                targets = newTargets;
                foreach (EnemyVisionManager enemy in enemyManagers)
                    if (enemy != null)
                        enemy.VisionUpdate();
            }
        }
    }

    public HashSet<GameObject> SensorUpdate()
    {
        HashSet<GameObject> detectedTargets = new HashSet<GameObject>();
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, visionRadius, visionLayer);
        foreach (var target in targetsInRange)
        {
            if (CheckTarget(target))
                detectedTargets.Add(target.gameObject);
        }
        return detectedTargets;
    }

    private bool CheckTarget(Collider target)
    {
        // check if in vision cone
        Vector3 vectorToTarget = (target.transform.position - transform.position).normalized;
        if (Vector3.Dot(vectorToTarget, transform.forward) < cosVisionConeAngle)
            return false;

        // raycast to target passes?
        RaycastHit hitResult;
        if (Physics.Raycast(transform.position, vectorToTarget, out hitResult, visionRadius))
        {
            if (hitResult.collider == target)
                return true;
        }
        return false;
    }

    public void RegisterEnemy(EnemyVisionManager enemy)
    {
        enemyManagers.Add(enemy);
    }

    public void DeregisterEnemy(EnemyVisionManager enemy)
    {
        enemyManagers.Remove(enemy);
    }

    private void OnDestroy()
    {
        if (SensorManager.Instance != null)
            SensorManager.Instance.DeregisterVisionSensor(gameObject);
    }
}