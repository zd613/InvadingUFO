using UnityEngine;


public class OnTriggerStaySender : MonoBehaviour
{

    public event System.Action<Collider> OnTriggerStayCalled;

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayCalled?.Invoke(other);
    }
}
