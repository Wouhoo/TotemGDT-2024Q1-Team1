using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ISignalReceiver
{
    // Called when the door is activated by e.g. a button (overrides default ButtonActivate from TriggerableObject)
    public void Signal(bool signal)
    {
        // Open door if it is closed & vice versa
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
