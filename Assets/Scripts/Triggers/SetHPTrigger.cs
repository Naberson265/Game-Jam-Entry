using UnityEngine;
using System.Collections.Generic;

public class SetHPTrigger : MonoBehaviour
{
    public List<int> hpSet = new List<int> { 0, 0, 0, 0, 0 };
    public bool activated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !activated)
        {
            PlayerController ps = PlayerController.playerController;
            if (ps.health != hpSet)
            {
                ps.health = hpSet;
                ps.playerAudio.PlayOneShot(ps.powerupSFX);
                ps.UpdateAppearance();
            }
        }
    }
}
