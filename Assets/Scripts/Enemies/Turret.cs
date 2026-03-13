using UnityEngine;

public class Turret : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 newLookAt = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            turretContainer.transform.LookAt(newLookAt);
            if (timeUntilShoot > 0f)
            {
                timeUntilShoot -= Time.deltaTime;
            }
            else
            {
                turretContainer.transform.LookAt(playerTransform.position);
                GameObject newBullet = Instantiate(bulletPrefab, transform.position + (transform.forward * 5f), turretTop.transform.rotation);
                newBullet.GetComponent<TurretBullet>().moveSpeed = bulletSpeed;
                timeUntilShoot = shotInterval;
                turretContainer.transform.LookAt(newLookAt);
            }
        }
    }
	public float shotInterval = 0.5f;
	private float timeUntilShoot;
	public float bulletSpeed = 20f;
	public GameObject bulletPrefab;
	public GameObject turretContainer;
	public GameObject turretTop;
	public Transform playerTransform;
}
