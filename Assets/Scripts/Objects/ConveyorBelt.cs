using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltScript : MonoBehaviour
{
    public float beltspeed;
    private float moveSpeed;
    public MeshRenderer meshRenderer;
    private Vector2 vector;
    public int offset, MoveOffset;
    private void Start()
    {
        moveSpeed = beltspeed / 4;
        meshRenderer = transform.parent.gameObject.GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (Time.timeScale != 0f)
        {
            if (offset == 0 || offset == 3) //forward belt
            {
                vector.y += beltspeed * Time.deltaTime;
                if (vector.y >= 20f)
                {
                    vector.y = 0f;
                }
                meshRenderer.material.SetTextureOffset("_MainTex", vector);
            }
            else //Back belt
            {
                vector.y -= beltspeed * Time.deltaTime;
                if (vector.y <= 0f)
                {
                    vector.y = 20f;
                }
                meshRenderer.material.SetTextureOffset("_MainTex", vector);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Vector3 addedVelocity = Vector3.zero;
        if (offset == 0) //forward
        {
            addedVelocity = new Vector3(1, 0, 0);
        }
        else if (offset == 1) //back
        {
            addedVelocity = new Vector3(-1, 0, 0);
        }
        else if (offset == 2) //left
        {
            addedVelocity = new Vector3(0, 0, 1);
        }
        else if (offset == 3) //right
        {
            addedVelocity = new Vector3(0, 0, -1);
        }
        if (other.transform.gameObject.layer == 3)
        {
            other.transform.position += addedVelocity * MoveOffset * moveSpeed;
        }
        if (other.transform.gameObject.layer == 7)
        {
            other.transform.position += addedVelocity * MoveOffset * moveSpeed;
        }
    }
}
