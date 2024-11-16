using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public List<SignalReceiver> signalReceivers; // List of receivers that this signal emitter triggers

    // Activate all receivers
    public void EmitSignal(bool signal)
    {
        foreach (SignalReceiver receiver in signalReceivers)
            receiver.Signal(gameObject, signal);
    }
}
