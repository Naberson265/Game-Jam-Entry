using UnityEngine;

public class PlayerDupe : MonoBehaviour
{
    void Update()
    {
        RaycastHit raycastHit;
        if (!Physics.Raycast(transform.position, -transform.up, out raycastHit, (transform.localScale.x / 2f), rayLayerMask, QueryTriggerInteraction.Ignore))
        {
            yVelocity -= ps.gravity * Time.deltaTime;
        }
        else yVelocity = 0f;
        transform.position += new Vector3 (0f, yVelocity * Time.deltaTime, 0f);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player") objCollider.enabled = true;
    }
    public float yVelocity;
	public LayerMask rayLayerMask;
    public Collider objCollider;
    public PlayerScript ps;
}
