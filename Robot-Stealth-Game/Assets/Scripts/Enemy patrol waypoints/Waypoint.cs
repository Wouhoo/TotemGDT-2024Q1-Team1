using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This is the base Waypoint class, other variants extend this one
This one has no fancy behavior, just a basic waypoint
*/

public class Waypoint : MonoBehaviour
{
    //might need this later for some reason
    //Not a fan of the fact that this is the type of a patrolling enemy, is confusing
    public EnemyAI_Patrol assignedEnemy;

    //upon arriving at this waypoint
    public virtual void OnReachWaypoint()
    {
        //nothing
    }

    void OnDrawGizmos()
    {
        //Show the waypoint in the editor only
        if (assignedEnemy != null)
        {
            //TODO: Gizmos.color = assignedEnemy.enemyColor; // Use a predefined color per enemy
            Gizmos.color = assignedEnemy.gizmoColor;
            Gizmos.DrawSphere(transform.position, 0.5f); // Draw a sphere for the waypoint in editor
        }
    }

    //upon arriving at next waypoint
    public virtual void OnExitWaypoint()
    {
        
    }
}
