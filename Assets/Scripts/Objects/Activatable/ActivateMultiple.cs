using UnityEngine;

public class ActivateMultiple : Activatable
{
    [SerializeField] private bool negated = false;
    [SerializeField] private Activatable[] activatables;
    private bool currentActivation;

    private void FixedUpdate()
    {
        if (activated != currentActivation)
        {
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = activated ^ negated;
            }
        }

        currentActivation = activated;
    }
}
