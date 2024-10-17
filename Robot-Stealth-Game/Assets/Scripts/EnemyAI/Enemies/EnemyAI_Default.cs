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
    public float memoryTime = 5f;
    [Header("Patrol State")]
    public float patrolSpeed;
    public Waypoint[] waypoints;

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
        var patrol = new State_Patrol(navMeshAgent, waypoints, patrolSpeed, this);
        var reset = new State_Look(this, 80f, navMeshAgent.angularSpeed);
        var inquire = new State_Inquire(sensorSystem, navMeshAgent, inquireSpeed, memoryTime, attackReach / 2, tickRate);
        var pursue = new State_Pursue(sensorSystem, navMeshAgent, pursueSpeed, memoryTime, attackReach / 2, tickRate);
        var attack = new EnemyState_Wait(5f);

        // TRANSITIONS
        At(patrol, inquire, InquireStart());
        At(patrol, pursue, PersueStart());

        At(reset, patrol, ResetDone());
        At(reset, inquire, InquireStart());
        At(reset, pursue, PersueStart());

        At(inquire, reset, InquireDone());
        At(inquire, pursue, PersueStart());

        At(pursue, reset, PursueDone());

        Any(attack, AttackStart());

        At(attack, pursue, AttackDone());

        // START STATE
        stateMachine.SetState(patrol);

        // CONDITIONS
        Func<bool> ResetDone() => () => reset.IsDone();

        Func<bool> InquireStart() => () => !sensorSystem.targetVisited;
        Func<bool> InquireDone() => () => inquire.IsDone();

        Func<bool> PersueStart() => () => sensorSystem.targetTransform != null;
        Func<bool> PursueDone() => () => pursue.IsDone();

        Func<bool> AttackStart() => () => sensorSystem.targetTransform != null && Vector3.Distance(transform.position, sensorSystem.targetTransform.position) < attackReach;
        Func<bool> AttackDone() => () => attack.IsDone();
    }



    void OnDrawGizmos()
    {
        //Draw this enemy's patrol paths in the editor ONLY
        if (waypoints != null && waypoints.Length > 1)
        {
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }
            //draw the last line back to the start
            Gizmos.DrawLine(waypoints[0].transform.position, waypoints[waypoints.Length - 1].transform.position);
        }
    }
}