using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int damageAmount = 1;
    public int damageLevel = 0;
    public bool ignoreIFrames = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController ps = other.transform.gameObject.GetComponent<PlayerController>();
            ps.Damage(damageAmount, damageLevel, ignoreIFrames);
        }
    }

}
