using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight_script : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;

    private float checkVisionInterval = 0.2f;

    // Implemented using layers for now, might need to refactor if some other script needs the layers more
    // The idea is that all player objects (robot body, hands, arm segments, anything that should be visible) are on the sightTarget layer,
    // while all obstacles that obstruct vision are on the obstacle layer.
    [SerializeField] LayerMask sightTargetMask;
    [SerializeField] LayerMask obstacleMask;

    void Start()
    {
        StartCoroutine("CheckVision", checkVisionInterval);
    }

    IEnumerator CheckVision(float interval)
    {
        // Checks if a target is in sight every <interval> seconds.
        while (true) // Might want to change this later to check if the enemy is blind/alive
        {
            yield return new WaitForSeconds(interval);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRadius, sightTargetMask); // Find all targets in view radius
        foreach (Collider target in targetsInRange)
        {
            Transform targetTransform = target.transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) // Check if target is within view cone/wedge
            {
                float distToTarget = Vector3.Distance(transform.position, targetTransform.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) // Cast ray to check if there are obstacles in the way
                {
                    // Target is in sight; do stuff here (possibly depending on what type of target we hit).
                    // For now, just print to the debug log
                    string objectName = target.name;
                    Debug.Log(objectName + " is in sight!");
                }
            }
        }
    }
}
