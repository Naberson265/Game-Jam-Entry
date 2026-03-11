using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrigger : Activatable
{
    public float setDensity = 0.015f;
    public Color setColor = Color.white;
    public bool fogState = true;
    private void FixedUpdate()
    {
        if (activated)
        {
            if (fogState)
            {
                GameController.gameController.SetFog(setDensity, setColor);
            }
            else
            {
                GameController.gameController.SetFog(0f, new Color(0f, 0f, 0f, 0f));
            }
            Destroy(gameObject);
        }
    }
}
