using UnityEngine;

public class DamageObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerScript ps = other.transform.gameObject.GetComponent<PlayerScript>();
            ps.Damage(damageAmount, ignoreIFrames, damageLevel);
        }
    }
    public float damageTime = 1.5f;
    public int damageAmount = 1;
    public int damageLevel = 0;
    public bool ignoreIFrames = false;
}
