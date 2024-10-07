using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// DO NOT EDIT!
/// This script registers the game object to the detectables manager.
/// This script should be added to things we want enemies to detect through sight or touch.
/// So add this to the player body, and arm components.
/// 
/// </summary>

public class DetectableTarget : MonoBehaviour
{
    private void Start()
    {
        DetectablesManager.Instance.RegisterTarget(gameObject);
    }

    private void OnDestroy()
    {
        if (DetectablesManager.Instance != null)
            DetectablesManager.Instance.DeregisterTarget(gameObject);
    }
}