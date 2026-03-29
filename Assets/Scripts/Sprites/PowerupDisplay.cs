using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDisplay : MonoBehaviour
{
    public Image[] powerQueueDisplays;

    // Update is called once per frame
    void Update()
    {
        List<int> health = PlayerController.playerController.health;

        for (int i = powerQueueDisplays.Length; i >= 0; i--)
        {
            if (i < health.Count)
            {
                powerQueueDisplays[health.Count - 1 - i].sprite = IconManager.iconManager.powerUpIcons[health[i]];
                powerQueueDisplays[health.Count - 1 - i].enabled = true;
            }
            else if (i < powerQueueDisplays.Length)
            {
                powerQueueDisplays[i].enabled = false;
            }
        }
    }
}
