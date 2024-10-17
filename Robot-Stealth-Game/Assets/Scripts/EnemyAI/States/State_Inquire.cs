using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Inquire : IState
{
    private readonly EnemySensorManager sensorSystem;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float inquireSpeed;
    private readonly float memoryTime; // Time spent pursuing a target which is in memory
    private readonly float minDist; // Reccommended to be half of the attacking distance (or at least less than)
    private readonly float tickRate;


    private float initialSpeed;
    private float memoryTimer = 0f;

    public State_Inquire(EnemySensorManager sensorSystem, NavMeshAgent navMeshAgent, float inquireSpeed, float memoryTime, float minDist, float tickRate)
    {
        this.sensorSystem = sensorSystem;
        this.navMeshAgent = navMeshAgent;
        this.inquireSpeed = inquireSpeed;
        this.memoryTime = memoryTime;
        this.minDist = minDist;
        this.tickRate = tickRate;
    }

    public void OnEnter()
    {
        Debug.Log("Inquire : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = inquireSpeed;
    }

    public void Tick()
    {
        // We update our vector target
        if (navMeshAgent.destination != sensorSystem.targetVector)
        {
            navMeshAgent.SetDestination(sensorSystem.targetVector);
            memoryTimer = 0f;
            return;
        }
        // Else we work on memory (timer counts to memory time)
        memoryTimer += tickRate;
    }

    public void OnExit()
    {
        Debug.Log("Inquire : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
    }

    public bool IsDone()
    {
        if (memoryTimer > memoryTime || navMeshAgent.remainingDistance < minDist)
        {
            // We leave this state via removing the target as nessesary to visit (Note we can also leave this state by other methods)
            sensorSystem.targetVisited = true;
            return true;
        }
        return false;
    }
}