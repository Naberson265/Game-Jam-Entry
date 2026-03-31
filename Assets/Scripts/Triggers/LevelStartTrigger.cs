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
    public Transform spawnpointPosition;
    // 0th is S, 1st is A, 2nd B and 3rd C. Any lower times are Ds (4th).
    public int[] rankTimes = {90, 120, 150, 180};
    // Recommend you use the below due to the level select system.
    public int levelToStart = -1;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (spawnpointPosition)
            {
                PlayerController.playerController.spawnpoint = spawnpointPosition.position;
            } else
            {
                PlayerController.playerController.spawnpoint = transform.position;
            }
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
            // If there is a designated level, set the level to 1 before it so that when
            // the GameController level function runs the correct level is set.
            // Also if you want to start level 3 enter 2. This is for consistency.
            if (levelToStart > -1) GameController.gameController.currentLevel = levelToStart - 1;
            GameController.gameController.StartNewLevel();
            Destroy(gameObject); // Make sure the player can't start a level multiple times using 1 trigger.
        }
    }
}
