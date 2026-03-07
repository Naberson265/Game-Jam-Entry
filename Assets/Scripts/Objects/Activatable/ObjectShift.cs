using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShift : Activatable
{
    public float pushSpeed = 0.1f;
    public Vector3 activeDisplacement;

    private Vector3 startPoint;
    private Vector3 activePoint;

    private void Start()
    {
        startPoint = transform.localPosition;
        activePoint = startPoint + activeDisplacement;
        if (activated)
        {
            transform.localPosition = activePoint;
        }
    }
    private void FixedUpdate()
    {
        Vector3 currentPos = transform.localPosition;
        if (activated)
        {
            if ((currentPos - activePoint).magnitude > 0.01f)
            {
                transform.localPosition = Vector3.Lerp(currentPos, activePoint, pushSpeed);
            }
            else
            {
                transform.localPosition = activePoint;
            }
        } else 
        {
            if ((currentPos - startPoint).magnitude > 0.01f)
            {
                transform.localPosition = Vector3.Lerp(currentPos, startPoint, pushSpeed);
            }
            else
            {
                transform.localPosition = startPoint;
            }
        }
    }
}
