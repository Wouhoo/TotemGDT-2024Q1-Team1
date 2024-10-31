using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : TriggerableObject
{
    GameObject playerArm;
    Material ventMaterial;
    [SerializeField] bool startOpen; // whether the vent is initially open or not

    private void Start()
    {
        playerArm = GameObject.Find("PlayerArm");
        ventMaterial = GetComponent<Renderer>().material;
        Trigger(startOpen);
    }

    // Called when the vent is activated by e.g. a button (overrides default Trigger from TriggerableObject)
    public override void Trigger(bool signal)
    {
        // Open vent if it is closed & vice versa
        if (signal)
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
