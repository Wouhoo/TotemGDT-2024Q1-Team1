using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class VisionSensor : MonoBehaviour
{
    [HideInInspector] public EnemyAI enemyAI;
    public float tickSpeed = 0.2f;
    [HideInInspector] public HashSet<GameObject> targets = new HashSet<GameObject>();
    [HideInInspector] public HashSet<EnemyVisionManager> enemyManagers = new HashSet<EnemyVisionManager>();
    public float visionConeAngle = 60f;
    public float visionConeRange = 30f;
    public Color visionConeColour = new Color(1f, 0f, 0f, 0.25f);

    public float cosVisionConeAngle { get; private set; } = 0f;

    [SerializeField] LayerMask detectionMask = ~0;
    private float tickDeadline;
    private void Awake()
    {
        // First let the sensor manager know this sensor exists
        SensorManager.Instance.RegisterVisionSensor(gameObject);

        cosVisionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);
    }

    private void Update()
    {
        if (Time.time >= tickDeadline)
        {
            tickDeadline = Time.time + tickSpeed;
            HashSet<GameObject> newTargets = SensorUpdate(DetectablesManager.Instance.AllTargets);
            if (!newTargets.SetEquals(targets))
            {
                targets = newTargets;
                foreach (EnemyVisionManager enemy in enemyManagers)
                    if (enemy != null)
                        enemy.VisionUpdate();
            }
        }
    }

    public HashSet<GameObject> SensorUpdate(HashSet<GameObject> candidateTargets)
    {
        HashSet<GameObject> detectedTargets = new HashSet<GameObject>();
        foreach (var target in candidateTargets)
        {
            if (CheckTarget(target))
                detectedTargets.Add(target);
        }
        return detectedTargets;
    }

    private bool CheckTarget(GameObject target)
    {
        // skip if the candidate is ourselves
        if (target.gameObject == gameObject)
            return false;
        var vectorToTarget = target.transform.position - transform.position;

        // if out of range - cannot see
        if (vectorToTarget.sqrMagnitude > (visionConeRange * visionConeRange))
            return false;
        vectorToTarget.Normalize();

        // if out of vision cone - cannot see
        if (Vector3.Dot(vectorToTarget, transform.forward) < cosVisionConeAngle)
            return false;

        // raycast to target passes?
        RaycastHit hitResult;
        if (Physics.Raycast(transform.position, vectorToTarget, out hitResult, visionConeRange, detectionMask, QueryTriggerInteraction.Collide))
        {
            if (hitResult.collider.gameObject == target)
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