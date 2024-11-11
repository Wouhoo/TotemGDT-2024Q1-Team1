using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : TriggerableObject
{
    GameObject playerArm;
    Material ventMaterial;
    public List<Trigger> triggers;
    [SerializeField] bool open;

    private void Start()
    {
        playerArm = GameObject.Find("PlayerArm");
        ventMaterial = GetComponent<Renderer>().material;
        if (open) OpenVent();
        else CloseVent();
    }

    // Called when the door is activated by e.g. a button (overrides default Trigger from TriggerableObject)
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
            OpenVent();
        else
            CloseVent();
    }

    void OpenVent()
    {
        // Opens the vent (so it is passable to the player arm - *still not to other gameobjects*)
        IgnoreArmCollision(true);
        SetAlpha(0.7f);
    }

    void CloseVent()
    {
        // Close the vent (so it is impassable to everything)
        IgnoreArmCollision(false);
        SetAlpha(1.0f);
    }

    void IgnoreArmCollision(bool collision)
    {
        // For each part of the player arm, set whether collisions with the vent should be ignored or not
        foreach (Transform child in playerArm.transform)
        {
            if (child.GetComponent<Collider>() != null)
                Physics.IgnoreCollision(GetComponent<Collider>(), child.GetComponent<Collider>(), collision);
        }
    }

    void SetAlpha(float alpha)
    {
        // Set alpha (transparency) of the vent
        ventMaterial.color = new Color(ventMaterial.color.r, ventMaterial.color.g, ventMaterial.color.b, alpha);
    }
}
