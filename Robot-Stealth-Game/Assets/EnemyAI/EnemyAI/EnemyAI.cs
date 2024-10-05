using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyAI : MonoBehaviour
{
    [HideInInspector] protected NavMeshAgent navMeshAgent;
    [HideInInspector] protected Animator animator;
    public float tickSpeed = 0.2f;

    protected StateMachine stateMachine = new StateMachine();
    private float tickDeadline;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update()
    {
        if (Time.time >= tickDeadline)
        {
            tickDeadline = Time.time + tickSpeed;
            stateMachine.Tick();
            Tick();
        }
    }

    protected virtual void Tick()
    {
        // Do nothing by default
    }
}
