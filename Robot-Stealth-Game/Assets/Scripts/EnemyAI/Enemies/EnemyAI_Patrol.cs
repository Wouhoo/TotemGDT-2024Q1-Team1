using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Patrol : EnemyAI
{
    [Header("Patrol")]
    public float patrolSpeed;
    //public Transform[] waypoints;
    public Waypoint[] waypoints;

    public Color gizmoColor;
    //workaround, unity is a bit dumb with uninitialized colors
    private bool isColorAssigned = false;

    private float pauseCounter = 0.0f;

    private void Start()
    {

        // STATES
        var patrol = new EnemyState_Patrol(navMeshAgent, waypoints, patrolSpeed, this);

        // TRANSITIONS

        // START STATE
        stateMachine.SetState(patrol);

        // CONDITIONS
    }


    public void PauseForSeconds(float duration)
    {
        Debug.Log("Pause waypoint: " + duration + " seconds");
        if (duration > 0.0f)
        {
            navMeshAgent.isStopped = true;
            pauseCounter = duration;
        }

    }

    protected override void Tick()
    {
        //update the pausecounter in case we are at a pause waypoint
        //TODO: would be nicer if handled in the waypoint itself maybe?
        //prevent underflow in the 1/100000 chance it might happen
        if (pauseCounter >= 0.0f)
        {
            pauseCounter -= Time.deltaTime;
            if (pauseCounter < 0.0f)
            {
                navMeshAgent.isStopped = false;
            }
        }
    }



    //draw lines in the editor for level design
    void OnDrawGizmos()
    {
        if (!isColorAssigned)
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