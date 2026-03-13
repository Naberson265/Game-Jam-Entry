using UnityEngine;

public class Turret : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 newLookAt = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            turretContainer.transform.LookAt(newLookAt);
            turretTop.transform.LookAt(playerTransform.position);
            if (timeUntilShoot > 0f)
            {
                timeUntilShoot -= Time.deltaTime;
            }
            else
            {
                turretTop.transform.LookAt(playerTransform.position);
                GameObject newBullet = Instantiate(bulletPrefab, transform.position + (transform.forward * 5f), transform.rotation);
                newBullet.GetComponent<TurretBullet>().moveSpeed = bulletSpeed;
                timeUntilShoot = shotInterval;
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
