using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Patrol : IState
{
    private readonly NavMeshAgent navMeshAgent;
    private readonly Transform[] waypoints;
    private readonly float patrolSpeed;
    private int waypointIndex;
    private float initialSpeed;

    public EnemyState_Patrol(NavMeshAgent navMeshAgent, Transform[] waypoints, float patrolSpeed)
    {
        this.navMeshAgent = navMeshAgent;
        this.waypoints = waypoints;
        this.patrolSpeed = patrolSpeed;
    }

    public void OnEnter()
    {
        Debug.Log("Patrol : Enter");
        // enemyReferences.navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = patrolSpeed;
    }

    public void Tick()
    {
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[waypointIndex].position);
        }
    }

    public void OnExit()
    {
        Debug.Log("Patrol : Exit");
        navMeshAgent.speed = initialSpeed;
        // enemyReferences.navMeshAgent.enabled = false;
    }
}