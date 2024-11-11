using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TriggerableObject
{
    [SerializeField] List<Trigger> triggers;
    [SerializeField] bool open;

    private void Start()
    {
        if(open) OpenDoor();
        else CloseDoor();
    }

    // Called when the door is activated by e.g. a button (overrides default Trigger from TriggerableObject)
    // Note: each door must keep a list of all buttons that trigger it, and each button keeps a list of all doors triggered by it.
    // I know that sounds like overkill, but that's the best I could do, for reasons that are a bit complicated to explain here.
    public override void Trigger()
    {
        // Check if *any* connected triggers are active
        bool _open = false;
        foreach (Trigger trigger in triggers)
        {
            if (trigger.active)
                _open = true;
        }
        if (_open == open) return; // State remains the same; no changes necessary
        // State changed; do stuff
        open = _open; 
        if (open)
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
