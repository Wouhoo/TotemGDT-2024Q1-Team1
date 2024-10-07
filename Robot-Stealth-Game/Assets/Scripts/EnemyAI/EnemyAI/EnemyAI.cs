using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
///  IT IS STRONGLY RECCOMENDED NOT EDIT THIS SCRIPT
///  This is the parent of all enemy scripts and hence has massive sweeping effects if changed!
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
        // We get all vital components
        // (note: this may all be moved into the individual enemy scripts in the futre for practical purposes)
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // We setup two curtial functions which make our code in the individual enemy scripts more legible
    protected void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update()
    {
        // All inherited classes should NEVER call Update()
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
