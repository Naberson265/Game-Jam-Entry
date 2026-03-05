using UnityEngine;

public class OneTimeTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;
    [SerializeField] private bool setActivationTo = true;
    private bool triggerActivated = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !triggerActivated)
        {
            foreach (Activatable activatable in activatables)
            {
                triggerActivated = true;
                activatable.activated = setActivationTo;
            }
        }
    }
}
