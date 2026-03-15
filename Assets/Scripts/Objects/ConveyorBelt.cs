using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltScript : MonoBehaviour
{
    public float beltspeed;
    private float moveSpeed;
    public MeshRenderer meshRenderer;
    private Vector2 vector;
    private void Start()
    {
        moveSpeed = beltspeed / 4;
        meshRenderer = transform.parent.gameObject.GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (Time.timeScale != 0f)
        {
            vector.y -= beltspeed * Time.deltaTime;
            if (vector.y >= 20f)
            {
                vector.y = 0f;
            }
            meshRenderer.material.SetTextureOffset("_MainTex", vector);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Vector3 addedVelocity = transform.forward;
        if (other.transform.gameObject.layer == 3)
        {
            Rigidbody otherRb = other.transform.gameObject.GetComponent<Rigidbody>();
            otherRb.position += addedVelocity * moveSpeed;
        }
        if (other.transform.gameObject.layer == 7)
        {
            other.transform.position += addedVelocity * moveSpeed;
        }
    }
}
