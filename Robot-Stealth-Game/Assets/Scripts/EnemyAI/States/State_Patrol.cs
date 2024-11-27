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
    private bool busy = false;
    private float rotationSpeed;
    public bool singleWaypointIsDone = false; // true iff there is only 1 waypoint and it has been visited

    public State_Patrol(NavMeshAgent navMeshAgent, Waypoint[] waypoints, float patrolSpeed, MonoBehaviour parent)
    {
        this.navMeshAgent = navMeshAgent;
        this.waypoints = waypoints;
        this.patrolSpeed = patrolSpeed;
        this.parent = parent;
        rotationSpeed = navMeshAgent.angularSpeed;
    }

    public void OnEnter()
    {
        Debug.Log("Patrol : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = patrolSpeed;
        busy = false;
        singleWaypointIsDone = false;

        TargetNearestWaypoint();
    }

    private void TargetNearestWaypoint()
    {
        float minDist = Mathf.Infinity;
        int minTarget = 0;
        Vector3 currentPos = parent.transform.position;
        for (int index = 0; index < waypoints.Length; index++)
        {
            float dist = Vector3.Distance(currentPos, waypoints[index].waypointPosition);
            if (dist < minDist)
            {
                minDist = dist;
                minTarget = index;
            }
        }
        waypointIndex = minTarget;
        navMeshAgent.SetDestination(waypoints[waypointIndex].waypointPosition);
    }

    public void Tick()
    {
        if (navMeshAgent.pathPending)
            return;

        if (navMeshAgent.remainingDistance > 0.1f)
            return;

        // catch stuck errors:
        if (0.15f < Vector3.Distance(parent.transform.position, waypoints[waypointIndex].waypointPosition))
        {
            TargetNearestWaypoint();
            return;
        }

        // Else we arrive at the waypoint; we are now busy until we have done all tasks here
        if (!busy)
        {
            busy = true;
            waitCoroutine = parent.StartCoroutine(ArriveAtWaypoint());
        }
    }

    private IEnumerator ArriveAtWaypoint()
    {
        // Face target direction
        float progress = 0f;
        Quaternion targetOrientation = Quaternion.Euler(0, waypoints[waypointIndex].facingAngle, 0);
        Quaternion currentOrientation = parent.transform.rotation;
        float angleDiff = Math.Abs(Quaternion.Angle(currentOrientation, targetOrientation));
        if (angleDiff > 0.1f) // else already facing right direction
        {
            float timeScale = rotationSpeed / angleDiff;
            while (progress < 1f)
            {
                progress += Time.deltaTime * timeScale;
                parent.transform.rotation = Quaternion.Slerp(currentOrientation, targetOrientation, progress);
                yield return null; // Wait until the next frame
            }
        }

        // We now wait for the designated pause time
        yield return new WaitForSeconds(waypoints[waypointIndex].pauseTime);

        // special case of only 1 waypoint:
        if (waypoints.Length == 1)
            singleWaypointIsDone = true;
        // We have completed all tasks, we now move on
        NextWaypoint();
        busy = false;
        parent.StopCoroutine(waitCoroutine);
    }

    private void NextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[waypointIndex].waypointPosition);
    }

    public void OnExit()
    {
        Debug.Log("Patrol : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
        busy = false;
        singleWaypointIsDone = false;

        if (waitCoroutine != null)
            parent.StopCoroutine(waitCoroutine);
    }
}