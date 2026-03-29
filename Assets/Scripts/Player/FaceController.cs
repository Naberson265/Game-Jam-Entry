using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField] private Transform pupil1;
    [SerializeField] private Transform pupil2;
    [SerializeField] private SpriteRenderer mouthSprite;
    [SerializeField] private SpriteRenderer[] pupilSprites;
    [SerializeField] private SpriteRenderer[] eyeSprites;
    [SerializeField] Transform lookTransform1;
    [SerializeField] Transform lookTransform2;

    [SerializeField] private float lookRange = 20f;
    [SerializeField] private float pupilSpeed = 0.1f;
    [SerializeField] private float pupilDistance = 0.1f;
    [SerializeField] private LayerMask objectsOfInterest;
    // Check the mouth spritesheet for the order of faces (left to right).
    public Sprite[] mouthSpriteList;
    // 0-Pupil 1-Small Pupil 2-Blink 3-Eye White
    public Sprite[] eyeSpriteList;

    private Vector3 lookLocation;

    // Update is called once per frame
    void Update()
    {
        PlayerController ps = PlayerController.playerController;
        mouthSprite.sprite = mouthSpriteList[ps.mouthState];
        foreach (SpriteRenderer sprRend in eyeSprites)
        {
            if (ps.eyeState == 2) sprRend.sprite = eyeSpriteList[2];
            else sprRend.sprite = eyeSpriteList[3];
        }
        foreach (SpriteRenderer sprRend2 in pupilSprites)
        {
            sprRend2.sprite = eyeSpriteList[ps.eyeState];
        }
    }
    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, lookRange, objectsOfInterest);

        float minDist = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider hitCollider in hitColliders)
        {
            float dist = (hitCollider.transform.position - transform.position).sqrMagnitude;

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
