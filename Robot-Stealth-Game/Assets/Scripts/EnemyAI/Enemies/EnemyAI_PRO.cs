using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_PRO : EnemyAI
{
    [HideInInspector] public VisionSensor localVisionSensor;
    public HashSet<VisionSensor> fixedVisionSensors = new HashSet<VisionSensor>();
    private VisionManager visionManager;

    private void Start()
    {
        localVisionSensor = GetComponent<VisionSensor>();
        visionManager = new VisionManager(transform, 2, 2);
        // TEMP:
        fixedVisionSensors.Add(localVisionSensor);
        visionManager.VisionListenersRefresh(fixedVisionSensors);

        // STATES
        var attack = new EnemyState_Attack(visionManager, navMeshAgent);
        var idle = new EnemyState_Idle();

        // TRANSITIONS
        At(attack, idle, AttackDone());
        Any(attack, CanSee());


        // START STATE
        stateMachine.SetState(idle);

        // CONDITIONS
        Func<bool> CanSee() => () => visionManager.targetTransform != null;
        Func<bool> AttackDone() => () => attack.IsDone();
    }

    protected override void Tick()
    {
        localVisionSensor.Tick();
        UpdateSensors();
    }

    private void UpdateSensors()
    {
        // new vision sensors : NEW ONES
        // update those:
        // visionManager.VisionListenersRefresh(NEW ONES);
    }
}