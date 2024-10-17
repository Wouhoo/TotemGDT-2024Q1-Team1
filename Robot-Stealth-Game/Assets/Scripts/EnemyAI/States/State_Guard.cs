using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class State_Guard : IState
{
    private NavMeshAgent navMeshAgent;
    private readonly Vector3 guardPosition;
    private MonoBehaviour parent;
    private readonly float guardFacingAngle;
    private readonly float rotationSpeed;
    private readonly float guardSpeed;
    private float realRotationSpeed;
    private Coroutine goGuard;
    private Quaternion currentOrientation;
    private Quaternion targetOrientation;

    private float initialSpeed;

    public State_Guard(NavMeshAgent navMeshAgent, Vector3 guardPosition, MonoBehaviour parent, float guardFacingAngle, float guardSpeed)
    {
        this.navMeshAgent = navMeshAgent;
        this.guardPosition = guardPosition;
        this.guardSpeed = guardSpeed;
        this.parent = parent;
        this.guardFacingAngle = guardFacingAngle;
        rotationSpeed = navMeshAgent.angularSpeed;
        targetOrientation = Quaternion.Euler(0, guardFacingAngle, 0);
    }

    public void OnEnter()
    {
        Debug.Log("Guard : Enter");
        navMeshAgent.enabled = true;
        initialSpeed = navMeshAgent.speed;
        navMeshAgent.speed = guardSpeed;

        navMeshAgent.SetDestination(guardPosition);
        goGuard = parent.StartCoroutine(GoGuard());
    }

    public void Tick()
    {
    }

    private IEnumerator GoGuard()
    {
        if (navMeshAgent.pathPending)
            yield return null; // I DONT KNOW WHY BUT THIS IS FULLY NEEDED (DO NOT REMOVE THIS LINE)
        while (navMeshAgent.remainingDistance > 0.1f)
        {
            yield return new WaitForSeconds(0.2f); // Wait until the next frame (with some delay)
        }
        navMeshAgent.enabled = false;

        float progress = 0f;

        realRotationSpeed = rotationSpeed / Math.Abs(Math.Abs(parent.transform.rotation.y * 180 / Mathf.PI) - Math.Abs(guardFacingAngle));
        Debug.Log(rotationSpeed);
        Debug.Log(parent.transform.rotation.y);
        Debug.Log(Math.Abs(parent.transform.rotation.y - guardFacingAngle));
        Debug.Log(realRotationSpeed);
        currentOrientation = parent.transform.rotation;

        while (progress < 1f)
        {
            progress += Time.deltaTime * realRotationSpeed;
            parent.transform.rotation = Quaternion.Slerp(currentOrientation, targetOrientation, progress);
            yield return null; // Wait until the next frame
        }
        parent.StopCoroutine(goGuard);
    }

    public void OnExit()
    {
        Debug.Log("Guard : Exit");
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.enabled = false;
        if (goGuard != null)
            parent.StopCoroutine(goGuard);
    }
}