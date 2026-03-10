using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        expireTime -= Time.deltaTime;
        if (expireTime < 0f) Destroy(gameObject);
    }
    public float moveSpeed = 20f;
    public float expireTime = 10f;
}
