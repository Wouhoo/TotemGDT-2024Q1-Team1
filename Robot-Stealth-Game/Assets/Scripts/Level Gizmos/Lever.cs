using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*

public class Lever : SignalEmitter
{
    void EmitSignal()
    {
        Debug.Log("lol");
        // Switch between active and inactive state
        active = !active;
        // Emit a signal to all receivers for this lever
        foreach (TriggerableObject receiver in signalReceivers)
        {
            receiver.Trigger();
        }
    }

    // When button is clicked
    public void OnPointerClick(PointerEventData eventData) 
    {
        Debug.Log("lmao");
        // Check if the player's hands or body are near the button
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, (transform.localScale / 2) * 1.1f, Quaternion.identity, playerMask);

        if (hitColliders.Length > 0)
        {
            // When button is pressed, activate all objects
            EmitSignal();
        }
    }
}

*/