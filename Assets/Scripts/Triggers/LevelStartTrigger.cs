using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelStartTrigger : MonoBehaviour
{
    public Activatable[] activatablesOn;
    public Activatable[] activatablesOff;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    public GameController gc;
    void OnTriggerEnter()
    {
        gc.StartNewLevel();
        foreach (GameObject activatingObj in objectsToActivate)
        {
            activatingObj.SetActive(true);
        }
        foreach (GameObject deactivatingObj in objectsToDeactivate)
        {
            deactivatingObj.SetActive(false);
        }
        foreach (Activatable actionObjectOn in activatablesOn)
        {
            actionObjectOn.activated = true;
        }
        foreach (Activatable actionObjectOff in activatablesOff)
        {
            actionObjectOff.activated = false;
        }
        Destroy(gameObject); // Make sure the player can't start a level multiple times using 1 trigger.
    }
}
