using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_Button : SignalEmitter
{
    public ConfigurableJoint joint;
    private int objectsOnButton = 0;

    // First thing gets on the button
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "armNode(Clone)") // ignore arms
            return;

        if (objectsOnButton == 0)
        {
            joint.anchor = new Vector3(0, 0.8f, 0);
            EmitSignal(true);
        }
        objectsOnButton++;
    }

    // Last thing gets off of the button
    private void OnTriggerExit(Collider collider)
    {
        if (collider.name == "armNode(Clone)") // ignore arms
            return;

        objectsOnButton--;
        if (objectsOnButton == 0)
        {
            joint.anchor = new Vector3(0, 0, 0);
            EmitSignal(false);
        }
    }
}
