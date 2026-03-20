using UnityEngine;
using System.Collections.Generic;

public class TeleporterTrigger : MonoBehaviour
{
    public Vector3 destinationPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            otherRb.position = destinationPos;
        }
    }
}
