using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
///  DO NOT EDIT THIS SCRIPT
///  
/// </summary>

[DisallowMultipleComponent]
public class EnemyAI : MonoBehaviour
{
    [HideInInspector] protected NavMeshAgent navMeshAgent;
    [HideInInspector] protected Animator animator;
    protected StateMachine stateMachine = new StateMachine();

    private float tickSpeed = 0.2f;
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
        // All inherited classes shoul NEVER call Update()
        // They MUST use Tick()
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
