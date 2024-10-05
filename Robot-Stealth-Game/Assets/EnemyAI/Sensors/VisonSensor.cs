using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class VisionSensor : MonoBehaviour
{
    [HideInInspector] public EnemyAI enemyAI;
    public bool unicast = false;
    public bool broadcast = false;
    public event Action OnVisionChange;
    public HashSet<GameObject> targets = new HashSet<GameObject>();
    // eyeLocation => transform.position;
    // eyeDirection => transform.forward;


    public float visionConeAngle = 60f;
    public float visionConeRange = 30f;
    public Color visionConeColour = new Color(1f, 0f, 0f, 0.25f);

    public float cosVisionConeAngle { get; private set; } = 0f;

    [SerializeField] LayerMask detectionMask = ~0;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        cosVisionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);
    }

    public void Tick()
    {
        HashSet<GameObject> newTargets = SensorUpdate(DetectablesManager.Instance.AllTargets);
        if (!newTargets.SetEquals(targets)) // TODO: is this correct? (!=) on a hashset
        {
            targets = newTargets;
            Debug.Log("BROADCASTING");
            if (broadcast) OnVisionChange?.Invoke();
            // TODO: if (unicast) enemyAI.
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


    // Make sure to clear all subscribers
    private void OnDisable()
    {
        OnVisionChange = null;
    }

    private void OnDestroy()
    {
        OnVisionChange = null;
    }
}