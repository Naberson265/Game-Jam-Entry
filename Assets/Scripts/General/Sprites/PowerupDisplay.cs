using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class powerUpDisplay : MonoBehaviour
{
    public Image[] powerQueueDisplays;

    // Update is called once per frame
    void Update()
    {
        List<int> health = PlayerController.playerController.health;
        int healthCheckIndex = health.Count - 1;
        int healthCheckIndexDisplay = 0;
        if (health.Count > 0)
        {
            foreach (Image display in powerQueueDisplays)
            {
                if (healthCheckIndexDisplay > 0)
                {
                    if (healthCheckIndexDisplay < health.Count)
                    {
                        powerQueueDisplays[healthCheckIndexDisplay].sprite = IconManager.iconManager.powerUpIcons[health[healthCheckIndex]];
                        powerQueueDisplays[healthCheckIndexDisplay].enabled = true;
                    }
                    else if (healthCheckIndexDisplay < powerQueueDisplays.Length)
                    {
                        powerQueueDisplays[healthCheckIndexDisplay].enabled = false;
                    }
                }
                else
                {
                    powerQueueDisplays[healthCheckIndexDisplay].sprite = IconManager.iconManager.powerUpIcons[0];
                }
                healthCheckIndex--;
                healthCheckIndexDisplay++;
            }
            powerQueueDisplays[health.Count - 1].sprite = IconManager.iconManager.powerUpIcons[health[0]];
        }
    }
}
