using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollision : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.transform.root.gameObject);
        print("destroy ,stage collision");

    }
}
