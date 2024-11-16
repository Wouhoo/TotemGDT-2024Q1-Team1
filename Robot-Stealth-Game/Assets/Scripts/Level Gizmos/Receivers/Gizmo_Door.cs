using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_Door : SignalReceiver
{
    public ConfigurableJoint joint;
    public Rigidbody closedPosition;
    public Rigidbody openPosition;

    private void Start()
    {
        joint.connectedBody = closedPosition;
    }

    // Opens the door
    protected override void Activate()
    {
        joint.connectedBody = openPosition;
    }

    // Closes the door
    protected override void Deactivate()
    {
        joint.connectedBody = closedPosition;
    }
}
