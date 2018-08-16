using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaAnim : MonoBehaviour
{

    int blendShapeCount;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    float blendOne = 0;
    float blendTwo = 0;

    public float blendSpeed = 1;

    // Use this for initialization
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = skinnedMeshRenderer.sharedMesh;
    }
    bool up = true;

    // Update is called once per frame
    void Update()
    {
        blendOne = Mathf.Clamp(blendOne, 0, 100);

        if (up)
        {
            blendOne += blendSpeed;
        }
        else
        {
            blendOne -= blendSpeed;
        }

        if (blendOne > 100) up = false;
        else if (blendOne < 0) up = true;

        skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);

    }
}
