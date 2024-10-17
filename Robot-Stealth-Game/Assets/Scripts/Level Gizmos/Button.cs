using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : SignalEmitter
{
    LayerMask playerMask;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }



    // rework this to also work if the player is on the button

    private void OnMouseDown()
    {
        // Check if the player's hands are near the button
        // For now, this checks if anything on the player layer is near the button. We may want to change this to only the hands later.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, (transform.localScale / 2) * 1.2f, Quaternion.identity, playerMask);

        if (hitColliders.Length > 0)
        {
            // When button is pressed, activate all objects
            EmitSignal(true);
        }
        else
            EmitSignal(false);
    }
}
