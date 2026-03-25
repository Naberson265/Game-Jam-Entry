using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : Activatable
{
    private Vector3 startPoint;
    private Quaternion startRotation;
    private Vector3 startScale;
    private Rigidbody objRB;

    public float timeTillFall = 0.5f;
    public float timeTillRespawn = 5f;

    private bool calledCoroutine = false;

    private void Start()
    {
        startPoint = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        objRB = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        if (activated && !calledCoroutine)
        {
            StartCoroutine(Fall());
            calledCoroutine = true;
        }
    }

    public void ResetObject()
    {
        StopAllCoroutines();
        transform.position = startPoint;
        transform.rotation = startRotation;
        transform.localScale = startScale;
        calledCoroutine = false;
        activated = false;
        objRB.isKinematic = true;
    }

    IEnumerator Fall()
    {
        for (int i = 0; i < 100; i++)
        {
            objRB.MovePosition(startPoint + new Vector3(Mathf.Sin(i * 0.2f) * 0.1f, 0, Mathf.Cos(i * 0.4f + 1f) * 0.1f));
            yield return new WaitForSeconds(0.005f);
        }
        if (timeTillFall - 0.5f > 0)
        {
            yield return new WaitForSeconds(timeTillFall - 0.5f);
        }

        objRB.isKinematic = false;
        objRB.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 300; i++)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = Vector3.zero;
        objRB.isKinematic = true;
        yield return new WaitForSeconds(timeTillRespawn - timeTillFall);
        transform.position = startPoint;
        transform.rotation = startRotation;
        for (int i = 0; i < 50; i++)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        transform.localScale = startScale;
        calledCoroutine = false;
        activated = false;
    }
}
