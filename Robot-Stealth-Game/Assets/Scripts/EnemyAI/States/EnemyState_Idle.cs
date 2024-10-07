using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Idle : IState
{
    private readonly NavMeshAgent navMeshAgent;
    private readonly float waitTime;

    private float deadLine;

    public EnemyState_Idle()
    {
    }

    public void OnEnter()
    {
        Debug.Log("Idle : Enter");
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
        Debug.Log("Idle : Exit");
    }

    public bool IsDone()
    {
        return Time.time >= deadLine;
    }
}