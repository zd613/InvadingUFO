using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public DestroyMode mode;
    public float delay = 4;

    private void Awake()
    {
        float d = 0;
        switch (mode)
        {
            case DestroyMode.Delay:
                d = delay;
                break;
            case DestroyMode.ParticleEnds:
                d = GetComponent<ParticleSystem>().main.duration;
                break;
            default:
                break;
        }
        Destroy(gameObject, d);
    }
}

public enum DestroyMode
{
    NoDelay,
    Delay,
    ParticleEnds,

}
