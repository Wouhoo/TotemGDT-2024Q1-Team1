using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Patrol : IState
{
    private readonly NavMeshAgent navMeshAgent;
    //private readonly Transform[] waypoints;
    private readonly Waypoint[] waypoints;
    private readonly float patrolSpeed;
    private int waypointIndex;
    private float initialSpeed;

    //reference back to the actual enemy
    private EnemyAI_Patrol assignedEnemy;

    public EnemyState_Patrol(NavMeshAgent navMeshAgent, Waypoint[] waypoints, float patrolSpeed, EnemyAI_Patrol enemy)
    {
        this.navMeshAgent = navMeshAgent;
        this.waypoints = waypoints;
        this.patrolSpeed = patrolSpeed;
        this.assignedEnemy = enemy;
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
            //call waypoint functions
            //Not a fan of this nesting but with this system I have no choice-Lars
            if(waypointIndex == 0)
            {
                waypoints[waypoints.Length-1].OnExitWaypoint();
            }
            else
            {
                waypoints[waypointIndex-1].OnExitWaypoint();
            }
            waypoints[waypointIndex].OnReachWaypoint();
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[waypointIndex].transform.position);
        }
    }

    public void OnExit()
    {
        Debug.Log("Patrol : Exit");
        navMeshAgent.speed = initialSpeed;
        // enemyReferences.navMeshAgent.enabled = false;
    }
}