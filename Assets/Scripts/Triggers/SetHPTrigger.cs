using UnityEngine;
using System.Collections.Generic;

public class SetHPTrigger : MonoBehaviour
{
    public List<int> hpSet = new List<int> { 0, 0, 0, 0, 0 };
    public bool usable = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && usable)
        {
            PlayerController ps = PlayerController.playerController;
            List<int> copySetList = new List<int>();
            copySetList.AddRange(hpSet);
            if (ps.health != hpSet)
            {
                ps.health = copySetList;
                ps.playerAudio.PlayOneShot(ps.powerupSFX);
                ps.UpdateAppearance();
            }
        }
    }
}
