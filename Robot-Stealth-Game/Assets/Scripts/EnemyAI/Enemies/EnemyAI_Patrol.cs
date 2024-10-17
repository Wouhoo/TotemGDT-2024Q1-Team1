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
public class EnemyAI_Patrol : EnemyAI
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI_Patrol))]
public class EnemyAI_Default_Editor : Editor
{
    void OnSceneGUI()
    {
        EnemyAI_Patrol enemy = (EnemyAI_Patrol)target;
        Handles.color = new Color(1f, 1f, 0f, 1f);
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 20;
        for (int i = 0; i < enemy.waypoints.Length; i++)
        {
            Handles.SphereHandleCap(0, enemy.waypoints[i].waypointPosition, Quaternion.identity, 1f, EventType.Repaint);
            Handles.Label(enemy.waypoints[i].waypointPosition, i.ToString(), labelStyle);
            Handles.DrawLine(enemy.waypoints[i].waypointPosition, enemy.waypoints[(i + 1) % enemy.waypoints.Length].waypointPosition);
        }
    }
}
#endif //UNITY_EDITOR

