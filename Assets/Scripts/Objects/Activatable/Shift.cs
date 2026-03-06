using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shift : Activatable
{
    public float doorSpeed = 0.1f;
    public Vector3 openDisplacement;

    private Vector3 closedPoint;
    private Vector3 openPoint;

    private void Start()
    {
        closedPoint = gameObject.transform.localPosition;
        openPoint = closedPoint + openDisplacement;
        if (activated)
        {
            gameObject.transform.localPosition = openPoint;
        }
    }
    private void FixedUpdate()
    {
        Vector3 currentPos = gameObject.transform.localPosition;
        if (activated)
        {
            if ((currentPos - openPoint).magnitude > 0.01f)
            {
                gameObject.transform.localPosition = Vector3.Lerp(currentPos, openPoint, doorSpeed);
            }
            else
            {
                gameObject.transform.localPosition = openPoint;
            }
        } else 
        {
            if ((currentPos - closedPoint).magnitude > 0.01f)
            {
                gameObject.transform.localPosition = Vector3.Lerp(currentPos, closedPoint, doorSpeed);
            }
            else
            {
                gameObject.transform.localPosition = closedPoint;
            }
        }
    }
}
