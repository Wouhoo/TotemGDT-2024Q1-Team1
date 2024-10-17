using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class State_Patrol : IState
{
    private readonly NavMeshAgent navMeshAgent;
    //private readonly Transform[] waypoints;
    private readonly Waypoint[] waypoints;
    private readonly float patrolSpeed;
    private int waypointIndex;
    private float initialSpeed;
    private MonoBehaviour parent;
    private Coroutine waitCoroutine;
    private bool inWaiting = false;

    public State_Patrol(NavMeshAgent navMeshAgent, Waypoint[] waypoints, float patrolSpeed, MonoBehaviour parent)
    {
        this.navMeshAgent = navMeshAgent;
        this.waypoints = waypoints;
        this.patrolSpeed = patrolSpeed;
        this.parent = parent;
    }

    public void OnEnter()
    {
        Debug.Log("Patrol : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = patrolSpeed;

        TargetNearestWaypoint();
    }

    private void TargetNearestWaypoint()
    {
        float minDist = Mathf.Infinity;
        int minTarget = 0;
        Vector3 currentPos = parent.transform.position;
        for (int index = 0; index < waypoints.Length; index++)
        {
            float dist = Vector3.Distance(currentPos, waypoints[index].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                minTarget = index;
            }
        }
        waypointIndex = minTarget;
    }

    public void Tick()
    {
        if (navMeshAgent.remainingDistance > 0.1f)
            return;

        // Else we arrive at the waypoint; if we are not currently waiting, we start waiting
        if (!inWaiting)
        {
            if (waypoints[waypointIndex].pauseTime < 0.1f) // Check if wait time is zero (use 0.1f for float errors)
            {
                NextWaypoint();
                return;
            }
            // else we begin the waiting routine
            inWaiting = true;
            waitCoroutine = parent.StartCoroutine(ArriveAtWaypoint());
        }
    }

    private IEnumerator ArriveAtWaypoint()
    {
        yield return new WaitForSeconds(waypoints[waypointIndex].pauseTime);
        //After we have waited for the given pause time we move on
        NextWaypoint();
        inWaiting = false;
        parent.StopCoroutine(waitCoroutine);
    }

    private void NextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[waypointIndex].transform.position);
    }

    public void OnExit()
    {
        Debug.Log("Patrol : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
        if (waitCoroutine != null)
            parent.StopCoroutine(waitCoroutine);
    }
}