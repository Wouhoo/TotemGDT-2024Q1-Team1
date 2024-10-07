using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Attack : IState
{
    private readonly VisionManager visionManager;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float attackMovmentSpeed;

    private float initialSpeed;

    public EnemyState_Attack(VisionManager visionManager, NavMeshAgent navMeshAgent)
    {
        this.visionManager = visionManager;
        this.navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        Debug.Log("Attack : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = 3.5f;
    }

    public void Tick()
    {
        if (visionManager.targetTransform != null)
            navMeshAgent.SetDestination(visionManager.targetTransform.position);
        else
            navMeshAgent.SetDestination(visionManager.targetVector);
    }

    public void OnExit()
    {
        Debug.Log("Attack : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
    }

    public bool IsDone()
    {
        return navMeshAgent.remainingDistance < 0.2f;
    }
}