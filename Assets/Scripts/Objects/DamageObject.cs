using UnityEngine;

public class DamageObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.gameObject.GetComponent<PlayerScript>().Damage(damageTime, damageAmount, ignoreIFrames);
        }
    }
    public float damageTime = 1.5f;
    public int damageAmount = 1;
    public bool ignoreIFrames = false;
}
