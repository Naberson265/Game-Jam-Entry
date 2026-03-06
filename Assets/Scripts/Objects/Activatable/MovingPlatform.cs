using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Activatable
{
    public bool isInitiallyActive;
    public float platformSpeed = 10f;
    public int currentTarget = 1;
    public Vector3[] platformDestination;
    // Make sure the first vector on the list is the platform origin.
    private Vector3 platformOrigin;

    private void Start()
    {
        platformOrigin = gameObject.transform.localPosition;
    }
    private void FixedUpdate()
    {
        Vector3 currentPos = gameObject.transform.localPosition;
        if (activated ^ isInitiallyActive)
        {
            if ((currentPos - platformDestination[currentTarget]).magnitude > 0.01f)
            {
                Rigidbody currentRb = GetComponent<Rigidbody>();
                currentRb.MovePosition(Vector3.MoveTowards(transform.position, platformDestination[currentTarget], platformSpeed * Time.fixedDeltaTime));
            }
            else
            {
                gameObject.transform.localPosition = platformDestination[currentTarget];
                if (currentTarget < platformDestination.Length - 1) currentTarget++;
                else currentTarget = 0;
            }
        }
    }
}
