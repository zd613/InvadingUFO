using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterSender : MonoBehaviour
{
    public event System.Action<Collider> OnTriggerEnterCalled;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterCalled?.Invoke(other);
    }
}
