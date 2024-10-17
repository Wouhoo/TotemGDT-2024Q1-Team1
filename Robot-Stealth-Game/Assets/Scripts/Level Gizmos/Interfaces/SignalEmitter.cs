using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public List<ISignalReceiver> signalRecievers;

    public void EmitSignal(bool signal)
    {
        foreach (ISignalReceiver receiver in signalRecievers)
            receiver.Signal(signal);
    }
}
