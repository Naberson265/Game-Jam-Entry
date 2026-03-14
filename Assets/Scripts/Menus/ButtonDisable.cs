using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisable : MonoBehaviour
{
    public int levelRequired;
    public int zoneRequired;
    private Button button;

    // This script disables buttons when the player hasn't progressed to a certain point
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (ProgressionManager._saveData.latestCheckpoint.zoneNum > zoneRequired ||
            (ProgressionManager._saveData.latestCheckpoint.zoneNum == zoneRequired && ProgressionManager._saveData.latestCheckpoint.levelNum >= levelRequired - 1))
        {
            button.interactable = true;
        } else
        {
            // TODO: Uncomment before release!
            //button.interactable = false;
        }
    }
}
