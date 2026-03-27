using UnityEngine;

public class FaceController : MonoBehaviour
{

    [SerializeField] private Transform pupil1;
    [SerializeField] private Transform pupil2;
    [SerializeField] Transform lookTransform1;
    [SerializeField] Transform lookTransform2;

    [SerializeField] private float lookRange = 20f;
    [SerializeField] private float pupilSpeed = 0.1f;
    [SerializeField] private float pupilDistance = 0.1f;
    [SerializeField] private LayerMask objectsOfInterest;

    private Vector3 lookLocation;

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, lookRange, objectsOfInterest);

        float minDist = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider hitCollider in hitColliders)
        {
            // Skip self if the character has a collider on the same layer
            if (hitCollider.gameObject == gameObject) continue;

            float dist = (hitCollider.transform.position - transform.position).sqrMagnitude; // Use sqrMagnitude for performance

            if (dist < minDist)
            {
                minDist = dist;
                closestTarget = hitCollider.transform;
            }
        }

        if(closestTarget)
        {
            lookLocation = Vector3.Lerp(lookLocation, closestTarget.position, pupilSpeed);
        } else
        {
            Vector3 defaultLocation = gameObject.transform.position + gameObject.transform.forward * 2;
            if ((lookLocation - defaultLocation).magnitude < 3)
            {

                lookLocation = defaultLocation;
            }
            lookLocation = Vector3.Lerp(lookLocation, gameObject.transform.position, pupilSpeed);
        }

        lookTransform1.LookAt(lookLocation);
        lookTransform2.LookAt(lookLocation);

        float magnitudeInY = Vector3.Dot(pupil1.up, lookTransform1.forward);
        float magnitudeInX = Vector3.Dot(pupil1.right, lookTransform1.forward);
        pupil1.localPosition = new Vector3(magnitudeInX, magnitudeInY, 0) * pupilDistance;

        magnitudeInY = Vector3.Dot(pupil2.up, lookTransform2.forward);
        magnitudeInX = Vector3.Dot(pupil2.right, lookTransform2.forward);
        pupil2.localPosition = new Vector3(magnitudeInX, magnitudeInY, 0) * pupilDistance;
    }
}
