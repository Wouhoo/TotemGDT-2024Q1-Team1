using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObject : MonoBehaviour
{
    // Class for objects that can be activated by e.g. a button or switch, such as doors.
    // These objects have a general method that can be called to *trigger* them (flip between some "off" and "on" state, or cycle through states).
    // This method is declared as virtual here. Any triggerable object should inherit from the TriggerableObject class and override this function.
    public virtual void ButtonActivate()
    {
        return;
    }
}