using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractableCapther : MonoBehaviour
{
    public event System.Action<Collider> OnCaptch;
    private void OnTriggerEnter(Collider other)
    {
        OnCaptch?.Invoke(other);
    }
}
