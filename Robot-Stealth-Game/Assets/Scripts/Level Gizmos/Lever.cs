using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lever : MonoBehaviour, IPointerClickHandler
{
    LayerMask playerMask;
    public List<TriggerableObject> signalReceivers;
    bool signal = true;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }

    void EmitSignal()
    {
        // Emit a signal to all receivers for this lever
        foreach (TriggerableObject receiver in signalReceivers)
        {
            receiver.Trigger(signal);
        }
        // Flip the "sign" of the signal for next time
        signal = !signal;
    }

    // When button is clicked
    public void OnPointerClick(PointerEventData eventData) 
    {
        // Check if the player's hands or body are near the button
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, (transform.localScale / 2) * 1.1f, Quaternion.identity, playerMask);

        if (hitColliders.Length > 0)
        {
            // When button is pressed, activate all objects
            EmitSignal();
        }
    }
}
