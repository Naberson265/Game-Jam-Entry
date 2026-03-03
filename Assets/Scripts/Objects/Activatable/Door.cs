using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    public bool isInitiallyOpen;
    public float doorSpeed = 0.1f;
    public Vector3 openDisplacement;


    private Vector3 closedPoint;
    private Vector3 openPoint;

    private void Start()
    {
        closedPoint = gameObject.transform.position;
        openPoint = closedPoint + openDisplacement;
        if (isInitiallyOpen)
        {
            gameObject.transform.position = openPoint;
        }
    }
    private void FixedUpdate()
    {
        Vector3 currentPos = gameObject.transform.position;
        if (activated ^ isInitiallyOpen)
        {
            if ((currentPos - openPoint).magnitude > 0.01f)
            {
                gameObject.transform.position = Vector3.Lerp(currentPos, openPoint, doorSpeed);
            }
            else
            {
                gameObject.transform.position = openPoint;
            }
        } else 
        {
            if ((currentPos - closedPoint).magnitude > 0.01f)
            {
                gameObject.transform.position = Vector3.Lerp(currentPos, closedPoint, doorSpeed);
            }
            else
            {
                gameObject.transform.position = closedPoint;
            }
        }
    }
}
