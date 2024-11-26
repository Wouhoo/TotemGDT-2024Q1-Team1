using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class State_Pursue : IState
{
    private readonly EnemySensorManager sensorSystem;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float pursueSpeed;
    private readonly float memoryTime; // Time spent pursuing a target which is in memory
    private readonly float minDist; // Reccommended to be half of the attacking distance (or at least less than)
    private readonly float tickRate;

    private float initialSpeed;
    private float memoryTimer = 0f;

    //just a quick way to get the music switching when the player is spotted
    //I know this is not the place for it but we got 2 days so yeah - Lars
    public static event Action OnPlayerSpotted;

    public State_Pursue(EnemySensorManager sensorSystem, NavMeshAgent navMeshAgent, float pursueSpeed, float memoryTime, float minDist, float tickRate)
    {
        this.sensorSystem = sensorSystem;
        this.navMeshAgent = navMeshAgent;
        this.pursueSpeed = pursueSpeed;
        this.memoryTime = memoryTime;
        this.minDist = minDist;
        this.tickRate = tickRate;
    }

    public void OnEnter()
    {
        Debug.Log("Pursue : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = pursueSpeed;
        OnPlayerSpotted?.Invoke();
    }

    public void Tick()
    {
        // Update target to the transform
        if (sensorSystem.targetTransform != null)
        {
            navMeshAgent.SetDestination(sensorSystem.targetTransform.position);
            memoryTimer = 0f;
            return;
        }
        // Else we work on memory (timer counts to memory time)
        memoryTimer += tickRate;
    }

    public void OnExit()
    {
        Debug.Log("Pursue : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
        memoryTimer = 0f;
    }

    public bool IsDone()
    {
        if (memoryTimer > memoryTime || navMeshAgent.remainingDistance < minDist)
        {
            // We leave this state via removing the target as nessesary to visit (Note we can also leave this state by moving to attack)
            sensorSystem.targetVisited = true;
            return true;
        }
        return false;
    }
}