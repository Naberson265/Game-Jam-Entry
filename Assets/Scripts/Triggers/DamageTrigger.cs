using UnityEngine;

public class DamageTrigger : Resettable
{
    public int damageAmount = 1;
    public int damageLevel = 0;
    public bool ignoreIFrames = false;
    public bool breakable = false;

    public bool broken = false;

    protected override void ResetObject()
    {
        gameObject.SetActive(!broken);
    }

    protected override void SaveDefault()
    {
        broken = !gameObject.activeSelf;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController ps = PlayerController.playerController;
            if (breakable)
            {
                if (damageLevel >= 3 ||
                damageLevel == 2 && !(ps.GetAbility() == 3 && ps.usedAirAbility) ||
                damageLevel == 1 && ps.GetAbility() != 3 ||
                damageLevel == 0 && ps.GetAbility() != 3 && ps.rb.linearVelocity.magnitude < 35f)
                {
                    ps.Damage(damageAmount, damageLevel, ignoreIFrames);
                }
                else
                {
                    ps.playerAudio.PlayOneShot(ps.spikeBreakSFX);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                ps.Damage(damageAmount, damageLevel, ignoreIFrames);
            }
        }
    }

}
