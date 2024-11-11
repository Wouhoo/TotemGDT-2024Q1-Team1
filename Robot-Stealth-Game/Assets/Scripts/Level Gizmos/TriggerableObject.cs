using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObject : MonoBehaviour
{
    // General class for objects that can be triggered, such as doors and vents.
    // Every such object should override the Trigger method below; when the object is triggered, it should look through all its triggers.
    // The TriggerableObject should be active/open if *at least one* trigger is active, and inactive/closed otherwise. 
    public virtual void Trigger()
    {
        return;
    }
}
