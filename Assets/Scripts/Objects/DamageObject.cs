using UnityEngine;

public class DamageObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.gameObject.GetComponent<PlayerScript>().Damage(damageTime, damageAmount, false);
        }
    }
    public float damageTime = 1.5f;
    public int damageAmount = 1;
}
