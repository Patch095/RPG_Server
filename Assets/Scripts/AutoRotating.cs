﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotating : MonoBehaviour
{
    float speed = 25f;

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }
}
