using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Patrol : EnemyAI
{
    [Header("Patrol")]
    public float patrolSpeed;
    public Transform[] waypoints;

    private void Start()
    {
        // STATES
        var patrol = new EnemyState_Patrol(navMeshAgent, waypoints, patrolSpeed);

        // TRANSITIONS

        // START STATE
        stateMachine.SetState(patrol);

        // CONDITIONS
    }
}