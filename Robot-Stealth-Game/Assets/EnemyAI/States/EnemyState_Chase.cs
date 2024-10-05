using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : IState
{
    private readonly NavMeshAgent navMeshAgent;
    private readonly Transform target;

    private float initialSpeed;

    public EnemyState_Chase(NavMeshAgent navMeshAgent, Transform target)
    {
        this.navMeshAgent = navMeshAgent;
        this.target = target;
    }

    public void OnEnter()
    {
        Debug.Log("Chase : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = 3.5f;
    }

    public void Tick()
    {
        navMeshAgent.SetDestination(target.position);
    }

    public void OnExit()
    {
        Debug.Log("Chase : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
    }

    public bool IsDone()
    {
        return navMeshAgent.remainingDistance < 0.2f;
    }
}