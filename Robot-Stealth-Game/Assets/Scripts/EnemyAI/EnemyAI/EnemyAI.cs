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

    protected float tickRate = 0.2f;
    public Color gizmoColor = Color.red;

    private void Awake()
    {
        // We get all vital components
        // (note: this may all be moved into the individual enemy scripts in the futre for practical purposes)
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        InvokeRepeating("Tick", tickRate, tickRate);
    }

    // We setup two curtial functions which make our code in the individual enemy scripts more legible
    protected void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);

    protected virtual void Tick()
    {
        stateMachine.Tick();
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
