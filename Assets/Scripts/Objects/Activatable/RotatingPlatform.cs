using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : Activatable
{
    public Vector3 rotateDir = new Vector3(0, 1, 0);
    private Rigidbody objRb;
    void Start()
    {
        objRb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (activated)
        {
            Quaternion deltaRotate = Quaternion.Euler(rotateDir * Time.fixedDeltaTime);
            objRb.rotation *= deltaRotate;
        }
    }
}
