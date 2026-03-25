using UnityEngine;
using System.Collections.Generic;

public class TeleporterTrigger : Activatable
{
    public Vector3 destinationPos;
    public GameObject[] glows;
    public bool multiUse = true;
    void Update()
    {
        if (activated)
        {
            foreach(GameObject glow in glows)
            {
                glow.SetActive(true);
            }
        }
        else
        {
            foreach(GameObject glow in glows)
            {
                glow.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() && activated)
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            otherRb.position = destinationPos;
        }
    }
}
