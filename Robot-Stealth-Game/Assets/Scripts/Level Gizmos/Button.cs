using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    LayerMask playerMask;
    public List<TriggerableObject> signalReceivers;
    int objectsOnButton = 0;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }

    void EmitSignal(bool signal)
    {
        active = signal;
        // Emit a signal to all receivers for this button
        foreach (TriggerableObject receiver in signalReceivers)
        {
            receiver.Trigger();
        }
    }

    // When another object (player *or enemy*) moves over the button, fire a true signal
    // This also fires when an arm *node* moves over the button; we don't want that.
    // This can be fixed by not giving the arm nodes colliders, or by specifically excluding them with a bit of code here.
    // (currently the second fix is implemented)
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "armNode(Clone)")
        {
            EmitSignal(true);
            objectsOnButton++;
        }
    }

    // When the other object moves off of the button, fire a false signal
    private void OnTriggerExit(Collider other)
    {
        if (other.name != "armNode(Clone)")
        {
            objectsOnButton--;
            if(objectsOnButton == 0)
                EmitSignal(false);
        }
    }
}
