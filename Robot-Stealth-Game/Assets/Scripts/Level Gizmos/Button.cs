using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public List<TriggerableObject> objectsToTrigger; // triggerable objects that should be activated by this button; set this in the editor
    LayerMask playerMask;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }

    private void OnMouseDown()
    {
        // Check if the player's hands are near the button
        // For now, this checks if anything on the player layer is near the button. We may want to change this to only the hands later.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, (transform.localScale / 2) * 1.2f, Quaternion.identity, playerMask);

        if(hitColliders.Length > 0)
        {
            // When button is pressed, activate all objects
            foreach (TriggerableObject obj in objectsToTrigger)
            {
                if (obj != null)
                {
                    obj.ButtonActivate();
                }
            }
        }
    }
}
