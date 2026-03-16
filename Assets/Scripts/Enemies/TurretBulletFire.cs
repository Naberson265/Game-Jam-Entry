using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        expireTime -= Time.deltaTime;
        if (expireTime < 0f) Destroy(gameObject);
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, moveSpeed / 5f, rayLayerMask, QueryTriggerInteraction.Ignore))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        GameObject hitObject = other.transform.gameObject;
        if (hitObject.layer == 3 || hitObject.layer == 6 || hitObject.layer == 7)
        {
            if (!other.isTrigger)
            {
                if (hitObject.name != "PhysicalCollider") Destroy(gameObject);
                else expireTime = 10f;
            }
        }
    }
	public LayerMask rayLayerMask;
    public float moveSpeed = 20f;
    public float expireTime = 10f;
}
