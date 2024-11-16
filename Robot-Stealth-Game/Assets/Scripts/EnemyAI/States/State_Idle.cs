using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Idle : IState
{
    public State_Idle()
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
}