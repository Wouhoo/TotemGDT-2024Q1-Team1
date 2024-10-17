using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This is the Waypoint class, only one enemy per waypoint,
*/

public class Waypoint : MonoBehaviour
{
    public EnemyAI assignedEnemy;
    public float pauseTime = 0f;

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
}
