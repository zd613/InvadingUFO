using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerExitSender : MonoBehaviour
{

    public event System.Action<Collider> OnTriggerExitCalled;

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitCalled?.Invoke(other);
    }
}
