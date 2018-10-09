using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public DestroyMode mode;
    [Header("mode をdelayに設定した時用")]
    public float delay = 4;

    private void Awake()
    {
        float delayTime = 0;
        switch (mode)
        {
            case DestroyMode.Delay:
                delayTime = delay;
                break;
            case DestroyMode.ParticleEnds:
                delayTime = GetComponent<ParticleSystem>().main.duration;
                break;
            case DestroyMode.SoundEnds:
                delayTime = GetComponent<AudioSource>().clip.length;
                break;
            default:
                break;
        }
        Destroy(gameObject, delayTime);
    }
}

public enum DestroyMode
{
    NoDelay,
    Delay,
    ParticleEnds,
    SoundEnds,

}
