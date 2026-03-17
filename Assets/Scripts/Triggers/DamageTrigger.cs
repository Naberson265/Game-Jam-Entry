using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int damageAmount = 1;
    public int damageLevel = 0;
    public bool ignoreIFrames = false;
    public bool breakable = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController ps = other.transform.gameObject.GetComponent<PlayerController>();
            ps.Damage(damageAmount, damageLevel, ignoreIFrames);
            if (breakable)
            {
                if (damageLevel >= 3 ||
                damageLevel == 2 && !(ps.GetAbility() == 3 && ps.usedAirAbility) ||
                damageLevel == 1 && ps.GetAbility() != 3)
                {
                    ps.playerAudio.PlayOneShot(ps.spikeBreakSFX);
                    Destroy(gameObject);
                }
            }
        }
    }

}
