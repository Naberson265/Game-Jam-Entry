using UnityEngine;

public class EveryTimeTrigger : MonoBehaviour
{
    [SerializeField] private bool playerOnly;
    [SerializeField] private Activatable[] activatables;
    [SerializeField] private bool setActivationTo = true;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3 || playerOnly == false)
        {
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = setActivationTo;
            }
        }
    }
}
