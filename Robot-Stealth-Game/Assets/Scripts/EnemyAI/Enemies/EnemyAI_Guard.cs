using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor; // see below

/// <summary>
/// This the basic default enemy
/// </summary>

[RequireComponent(typeof(EnemySensorManager))]
public class EnemyAI_Guard : EnemyAI
{
    public float memoryTime = 5f;
    [Header("Guard State")]
    public float guardSpeed = 5f;
    public Vector3 guardPosition;
    public float guardFacingAngle;

    [Header("Look Around State")]

    [Header("Inquire State")]
    public float inquireSpeed = 5f;

    [Header("Pursue State")]
    public float pursueSpeed = 10f;

    [Header("Attack State")]
    public float attackReach = 0.2f;



    private EnemySensorManager sensorSystem;

    private void Start()
    {
        sensorSystem = GetComponent<EnemySensorManager>();

        // STATES
        var guard = new State_Guard(navMeshAgent, guardPosition, this, guardFacingAngle, guardSpeed);
        var reset = new State_Look(this, 80f, navMeshAgent.angularSpeed);
        var inquire = new State_Inquire(sensorSystem, navMeshAgent, inquireSpeed, memoryTime, attackReach / 2, tickRate);
        var pursue = new State_Pursue(sensorSystem, navMeshAgent, pursueSpeed, memoryTime, attackReach / 2, tickRate);
        var attack = new EnemyState_Wait(5f);

        // TRANSITIONS
        At(guard, inquire, InquireStart());
        At(guard, pursue, PersueStart());

        At(reset, guard, ResetDone());
        At(reset, inquire, InquireStart());
        At(reset, pursue, PersueStart());

        At(inquire, reset, InquireDone());
        At(inquire, pursue, PersueStart());

        At(pursue, reset, PursueDone());

        Any(attack, AttackStart());

        At(attack, pursue, AttackDone());

        // START STATE
        stateMachine.SetState(guard);

        // CONDITIONS
        Func<bool> ResetDone() => () => reset.IsDone();

        Func<bool> InquireStart() => () => !sensorSystem.targetVisited;
        Func<bool> InquireDone() => () => inquire.IsDone();

        Func<bool> PersueStart() => () => sensorSystem.targetTransform != null;
        Func<bool> PursueDone() => () => pursue.IsDone();

        Func<bool> AttackStart() => () => sensorSystem.targetTransform != null && Vector3.Distance(transform.position, sensorSystem.targetTransform.position) < attackReach;
        Func<bool> AttackDone() => () => attack.IsDone();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI_Guard))]
public class EnemyAI_Guard_Editor : Editor
{
    void OnSceneGUI()
    {
        EnemyAI_Guard enemy = (EnemyAI_Guard)target;
        Handles.color = new Color(1f, 1f, 0f, 1f);
        Handles.SphereHandleCap(0, enemy.guardPosition, Quaternion.identity, 1f, EventType.Repaint);
    }
}
#endif //UNITY_EDITOR

