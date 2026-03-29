using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Activatable
{
    private Vector3 savedPosition;
    private int savedCurrentTarget;


    public float platformSpeed = 10f;
    public int currentTarget = 1;
    public Vector3[] platformDestination;

    private Vector3 platformOrigin;
    private Vector3 lastPosition;
    private Rigidbody playerRb;
    private bool playerOnPlatform = false;
    private Rigidbody rb;

    private void Start()
    {
        platformOrigin = transform.localPosition;
        lastPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        savedPosition = rb.position;
        savedCurrentTarget = currentTarget;
    }

    private void FixedUpdate()
    {
        Vector3 platformDelta = Vector3.zero;

        if (activated && platformDestination.Length > 0)
        {
            Vector3 targetWorldPos = transform.parent != null ?
                transform.parent.TransformPoint(platformDestination[currentTarget]) :
                platformOrigin + platformDestination[currentTarget];

            Vector3 newPos = Vector3.MoveTowards(transform.position, targetWorldPos, platformSpeed * Time.fixedDeltaTime);
            platformDelta = newPos - transform.position;
            rb.MovePosition(newPos);

            if ((newPos - targetWorldPos).sqrMagnitude < 0.001f)
            {
                currentTarget = (currentTarget + 1) % platformDestination.Length;
            }
        }

        if (playerOnPlatform && playerRb != null)
        {
            Vector3 move = new Vector3(platformDelta.x, 0f, platformDelta.z);
            playerRb.position += move;
        }

        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerRb = collision.rigidbody;
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerRb = null;
            playerOnPlatform = false;
        }
    }
    protected override void ResetObject()
    {
        transform.position = savedPosition;
        currentTarget = savedCurrentTarget;
        activated = storedActivated;
    }

    protected override void SaveDefault()
    {
        savedPosition = transform.position;
        savedCurrentTarget = currentTarget;
        storedActivated = activated;
    }
}