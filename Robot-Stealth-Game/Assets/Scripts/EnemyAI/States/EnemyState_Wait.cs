using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Wait : IState
{
    private readonly float waitTime;

    private float deadLine;

    public EnemyState_Wait(float waitTime)
    {
        this.waitTime = waitTime;
    }

    public void OnEnter()
    {
        Debug.Log("Wait : Enter");
        deadLine = Time.time + waitTime;
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
        Debug.Log("Wait : Exit");
    }

    public bool IsDone()
    {
        return Time.time >= deadLine;
    }
}