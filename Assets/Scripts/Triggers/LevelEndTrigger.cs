using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    // Make sure that newPlayerPos is directly touching a floor surface.
    // It moves to account for health. Same for newCamPos.
    public Transform newPlayerPos;
    public Transform newCamPos;
    // 0th is S, 1st is A, 2nd B and 3rd C. Any lower times are Ds (4th).
    public int[] rankTimes = {90, 120, 150, 180};
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Resettable.SaveDefaults();
            if (GameController.gameController.timePassed > 1f)
            {
                if (GameController.gameController.timePassed <= rankTimes[3])
                {
                    if (GameController.gameController.timePassed <= rankTimes[2])
                    {
                        if (GameController.gameController.timePassed <= rankTimes[1])
                        {
                            if (GameController.gameController.timePassed <= rankTimes[0])
                            {
                                GameController.gameController.levelRanks.Add(0);
                            }
                            else GameController.gameController.levelRanks.Add(1);
                        }
                        else GameController.gameController.levelRanks.Add(2);
                    }
                    else GameController.gameController.levelRanks.Add(3);
                }
                else GameController.gameController.levelRanks.Add(4);
            }
            newPlayerPos.position += new Vector3(0f, PlayerController.playerController.healthToSize[
            PlayerController.playerController.health.Count] / 2f, 0f);
            newCamPos.position += new Vector3(0f, PlayerController.playerController.healthToSize[
            PlayerController.playerController.health.Count] / 2f, 0f);
            GameController.gameController.EndLevelSet(newCamPos, newPlayerPos);
            Destroy(gameObject); // Make sure the player can't end a zone multiple times using 1 trigger.
        }
    }
}
