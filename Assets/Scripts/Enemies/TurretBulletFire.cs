using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        expireTime -= Time.deltaTime;
        if (expireTime < 0f) Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.transform.gameObject;
        if (hitObject.layer == 3 || hitObject.layer == 6 || hitObject.layer == 7)
        {
            if (!other.isTrigger)
            {
                if (hitObject.name != "PhysicalCollider") expireTime = 0f;
                else expireTime = 10f;
            }
        }
    }
    public float moveSpeed = 20f;
    public float expireTime = 10f;
}
