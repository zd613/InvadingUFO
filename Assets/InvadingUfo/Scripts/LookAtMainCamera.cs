using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{



    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        //transform.rotation = Quaternion.Inverse(transform.rotation);
    }
}
