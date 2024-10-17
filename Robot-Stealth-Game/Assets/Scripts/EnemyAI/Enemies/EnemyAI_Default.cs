using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This the basic default enemy
/// </summary>

[RequireComponent(typeof(EnemySensorManager))]
public class EnemyAI_Default : EnemyAI
{
    public float attackReach = 0.2f;
    public float memoryTime = 5f;
    public float pursueSpeed = 10f;
    public float inquireSpeed = 5f;

    private EnemySensorManager sensorSystem;

    private void Start()
    {
        sensorSystem = GetComponent<EnemySensorManager>();

        // STATES
        var normal = new EnemyState_Idle();
        var reset = new State_Look(this, 80f, navMeshAgent.angularSpeed);
        var inquire = new State_Inquire(sensorSystem, navMeshAgent, inquireSpeed, memoryTime, attackReach / 2, tickRate);
        var pursue = new State_Pursue(sensorSystem, navMeshAgent, pursueSpeed, memoryTime, attackReach / 2, tickRate);
        var action = new EnemyState_Wait(5f);

        // TRANSITIONS
        At(normal, inquire, InquireStart());
        At(normal, pursue, PersueStart());

        At(reset, normal, ResetDone());
        At(reset, inquire, InquireStart());
        At(reset, pursue, PersueStart());

        At(inquire, reset, InquireDone());
        At(inquire, pursue, PersueStart());

        At(pursue, reset, PursueDone());

        Any(action, AttackStart());

        At(action, pursue, AttackDone());

        // START STATE
        stateMachine.SetState(normal);

        // CONDITIONS
        Func<bool> ResetDone() => () => reset.IsDone();

        Func<bool> InquireStart() => () => !sensorSystem.targetVisited;
        Func<bool> InquireDone() => () => inquire.IsDone();

        Func<bool> PersueStart() => () => sensorSystem.targetTransform != null;
        Func<bool> PursueDone() => () => pursue.IsDone();

        Func<bool> AttackStart() => () => sensorSystem.targetTransform != null && Vector3.Distance(transform.position, sensorSystem.targetTransform.position) < attackReach;
        Func<bool> AttackDone() => () => normal.IsDone();
    }
}