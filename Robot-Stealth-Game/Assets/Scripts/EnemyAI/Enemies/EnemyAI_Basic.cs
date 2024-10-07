using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Basic : EnemyAI
{
    public Transform target;
    public float waitTime;

    private void Start()
    {
        // STATES
        var chase = new EnemyState_Chase(navMeshAgent, target);
        var wait = new EnemyState_Wait(navMeshAgent, waitTime);

        // TRANSITIONS
        At(chase, wait, ChaseDone());
        At(wait, chase, IdleDone());

        // START STATE
        stateMachine.SetState(wait);

        // CONDITIONS
        Func<bool> ChaseDone() => () => chase.IsDone();
        Func<bool> IdleDone() => () => wait.IsDone();
    }
}