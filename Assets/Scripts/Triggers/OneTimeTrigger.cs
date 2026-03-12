using UnityEngine;

public class OneTimeTrigger : Resettable
{
    [SerializeField] private Activatable[] activatables;
    [SerializeField] private bool setActivationTo = true;
    private bool triggerActivated = false;

    private bool defaultActivation = false;

    protected override void ResetObject()
    {
        triggerActivated = defaultActivation;

        // Reset Activatable if trigger is being disabled
        if (!triggerActivated)
        {
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = !setActivationTo;
            }
        }
    }

    protected override void SaveDefault()
    {
        defaultActivation = triggerActivated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !triggerActivated)
        {
            triggerActivated = true;
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = setActivationTo;
            }
        }
    }
}
