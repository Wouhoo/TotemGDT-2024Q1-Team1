using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SignalReceiver : MonoBehaviour
{
    // General class for objects that can be triggered, such as doors and vents.
    // Every such object should override the Trigger method below; when the object is triggered, it should look through all its triggers.
    // The TriggerableObject should be active/open if *at least one* trigger is active, and inactive/closed otherwise. 

    public bool initialState = false;
    private HashSet<GameObject> activeTriggers = new HashSet<GameObject>();
    protected bool state;

    private void Awake()
    {
        state = initialState;
    }

    public void Signal(GameObject gameObject, bool signal)
    {
        if (signal)
            activeTriggers.Add(gameObject);
        else
            activeTriggers.Remove(gameObject);
        UpdateState();
    }

    private void UpdateState()
    {
        bool newState = activeTriggers.Count != 0;

        if (newState == state)
            return; // No change in state

        if (newState)
            Activate();
        else
            Deactivate();
        state = newState;
    }

    protected virtual void Activate()
    { }
    protected virtual void Deactivate()
    { }
}
