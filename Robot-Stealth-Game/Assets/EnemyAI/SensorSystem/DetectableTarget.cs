using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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