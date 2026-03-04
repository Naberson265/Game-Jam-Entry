using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;

    private void OnTriggerEnter(Collider other)
    {
        foreach (Activatable activatable in activatables)
        {
            activatable.activated = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (Activatable activatable in activatables)
        {
            activatable.activated = false;
        }
    }
}
