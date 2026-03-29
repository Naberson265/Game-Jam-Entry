using UnityEngine;
using System.Collections;

public class DitherTransition : MonoBehaviour
{
    private Animator animController;
    void Start()
    {
        animController = GetComponent<Animator>();
    }
    public void StartAnim(string animToStart)
    {
        // Possible anims are Start and End. Idle doesn't have or need a trigger.
        animController.SetTrigger(animToStart);
    }
}
