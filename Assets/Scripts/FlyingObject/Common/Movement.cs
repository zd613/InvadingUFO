﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool isActive = true;

    [Header("パラメータ")]
    public float speed;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move()
    {
        if (!isActive)
        {
            return;
        }
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
}
