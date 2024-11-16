using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This is the Waypoint type

Order of opperations: go to position, turn to face direction, wait - then next waypoint.
*/

[System.Serializable]
public struct Waypoint
{
    public Vector3 waypointPosition;
    public float facingAngle;
    public float pauseTime;
}

