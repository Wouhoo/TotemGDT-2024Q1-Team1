using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySensorManager))]
[DisallowMultipleComponent]
public class HearingSensor : MonoBehaviour
{
    public float hearingRange;
    private EnemySensorManager enemy;

    private void Start()
    {
        // Register this sensor with the sensor manager
        SensorManager.Instance.hearingSensors.Add(this);
        enemy = GetComponent<EnemySensorManager>();
    }

    public void SensorUpdate(Vector3 source)
    {
        // Outside of hearing range
        if (Vector3.Distance(source, transform.position) > hearingRange)
            return;
        // Else inform all enemy
        enemy.VectorUpdate(source);
    }

    private void OnDestroy()
    {
        SensorManager.Instance.hearingSensors.Remove(this);
    }
}