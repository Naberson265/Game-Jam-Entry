using System;
using UnityEngine;

public class breakableBlockScript : Resettable
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController script = other.gameObject.GetComponent<PlayerController>();
            if (script != null)
            {
                if (script.slamingDowm)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    protected override void ResetObject()
    {
        gameObject.SetActive(true);
    }

    protected override void SaveDefault()
    {
        //not used
    }
}
