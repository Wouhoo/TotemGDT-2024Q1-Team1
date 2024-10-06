using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Patrol : EnemyAI
{
    [Header("Patrol")]
    public float patrolSpeed;
    public Transform[] waypoints;

    public Color gizmoColor;
    //workaround, unity is a bit dumb with uninitialized colors
    private bool isColorAssigned = false;

    private void Start()
    {

        // STATES
        var patrol = new EnemyState_Patrol(navMeshAgent, waypoints, patrolSpeed);

        // TRANSITIONS

        // START STATE
        stateMachine.SetState(patrol);

        // CONDITIONS
    }

    void OnDrawGizmos()
    {
        if(!isColorAssigned)
        {
            isColorAssigned = true;
            //get a color to show in the editor
            gizmoColor = GizmoColorManager.GetNextColor();
        }

        //Draw this enemy's patrol paths in the editor ONLY
        if (waypoints != null && waypoints.Length > 1)
        {
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }
            //draw the last line back to the start
            Gizmos.DrawLine(waypoints[0].transform.position, waypoints[waypoints.Length - 1].transform.position);
        }
    }
}