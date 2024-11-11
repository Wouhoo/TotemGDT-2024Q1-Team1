using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // General class for objects that can trigger other objects, such as buttons and levers.
    // Each trigger is either active or inactive; when its state changes, it should activate the Trigger() function
    // of each attached TriggerableObject to communicate this change.
    public bool active;
}
