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
            objRb.MoveRotation(objRb.rotation * deltaRotate);
            foreach (Rigidbody box in boxes)
            {
                Vector3 dir = box.position - objRb.position;
                dir = deltaRotate * dir;
                Vector3 newPos = objRb.position + dir;

                box.MovePosition(newPos);
                box.MoveRotation(deltaRotate * box.rotation);
            }
        }
    }
    
    private List<Rigidbody> boxes = new List<Rigidbody>();

    private void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = other.rigidbody;
        if (rb != null && other.gameObject.CompareTag("Player"))
        {
            boxes.Add(rb);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Rigidbody rb = other.rigidbody;
        if (rb != null && other.gameObject.CompareTag("Player"))
        {
            boxes.Remove(rb);
        }
    }
}
