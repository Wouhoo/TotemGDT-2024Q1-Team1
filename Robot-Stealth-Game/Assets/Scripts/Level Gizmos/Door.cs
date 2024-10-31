using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TriggerableObject
{
    // Called when the door is activated by e.g. a button (overrides default Trigger from TriggerableObject)
    public override void Trigger(bool signal)
    {
        // Open door if it receives a signal to open
        if (signal)
            OpenDoor();
        else
            CloseDoor();
    }

    // Opens the door
    void OpenDoor()
    {
        // Maybe add some cool animation later
        gameObject.SetActive(false);
    }

    // Close the door
    void CloseDoor()
    {
        gameObject.SetActive(true);
    }
}
