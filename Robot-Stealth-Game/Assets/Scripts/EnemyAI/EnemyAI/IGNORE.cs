using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IGNORE: i just didnt want to delete this look at target script in case we want to use it later elsewhere

public class Enemy_Actor : MonoBehaviour
{
    public Transform target;
    private void LookAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
