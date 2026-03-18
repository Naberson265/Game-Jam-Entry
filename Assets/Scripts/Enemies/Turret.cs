using UnityEngine;

public class Turret : MonoBehaviour
{

    private void Start()
    {
        playerTransform = PlayerController.playerController.transform;
        lookPosition = transform.position + transform.forward * 5;
        turretTop.transform.LookAt(lookPosition);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            lookPosition = Vector3.Lerp(lookPosition, playerTransform.position, 0.2f);
            turretTop.transform.LookAt(lookPosition);
            if (timeUntilShoot > 0f)
            {
                timeUntilShoot -= Time.deltaTime;
            }
            else if((lookPosition - playerTransform.position).magnitude < 3)
            {
                GameObject newBullet = Instantiate(bulletPrefab, turretTop.transform.position + (turretTop.transform.forward * 5f), turretTop.transform.rotation);
                newBullet.GetComponent<TurretBullet>().moveSpeed = bulletSpeed;
                timeUntilShoot = shotInterval;
            }
        }
    }
	public float shotInterval = 0.5f;
	private float timeUntilShoot;
	public float bulletSpeed = 20f;
	public GameObject bulletPrefab;
    public GameObject turretTop;
    private Transform playerTransform;
    private Vector3 lookPosition;
}
