using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelStartTrigger : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    void OnTriggerEnter()
    {
        GameController.StartNewLevel();
        foreach (GameObject activatingObj in objectsToActivate)
        {
            activatingObj.SetActive(true);
        }
        foreach (GameObject deactivatingObj in objectsToDeactivate)
        {
            deactivatingObj.SetActive(false);
        }
        Destroy(gameObject); // Make sure the player can't start a level multiple times using 1 trigger.
    }
}
