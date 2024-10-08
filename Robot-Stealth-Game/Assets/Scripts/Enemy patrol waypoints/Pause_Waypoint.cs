using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Makes the enemy pause upon arrival for specified time
*/

public class Pause_Waypoint : Waypoint
{
    public float duration;

    // Start is called before the first frame update
    public override void OnReachWaypoint()
    {
        assignedEnemy.PauseForSeconds(duration);
    }
}
