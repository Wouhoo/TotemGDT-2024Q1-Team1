using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_Switch : SignalEmitter
{
    public bool initialState;
    public ConfigurableJoint joint;
    private int objectsOnButton = 0;
    private bool isActive;
    public Material materialActive;
    public Material materialInactive;
    public MeshRenderer mr;

    private void Start()
    {
        mr.material = materialInactive;
        isActive = initialState;
        EmitSignal(isActive);
        if (isActive)
            mr.material = materialActive;
        else
            mr.material = materialInactive;
    }

    // First thing gets on the button
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "armNode(Clone)") // ignore arms
            return;

        if (objectsOnButton == 0)
        {
            isActive = !isActive;
            joint.anchor = new Vector3(0, 0.4f, 0);
            EmitSignal(isActive);
            if (isActive)
                mr.material = materialActive;
            else
                mr.material = materialInactive;
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
            joint.anchor = new Vector3(0, 0, 0);
    }
}
