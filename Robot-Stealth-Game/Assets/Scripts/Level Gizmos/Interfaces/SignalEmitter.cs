using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public List<ISignalReceiver> signalReceivers; // List of receivers that this signal emitter activates

    // Activate all receivers
    public void EmitSignal(bool signal)
    {
        foreach (ISignalReceiver receiver in signalReceivers)
            receiver.Signal(signal);
    }
}
