using UnityEngine;

public class PowerUpSpecificAM : Activatable
{
    [SerializeField] private bool negated = false;
    [SerializeField] private Activatable[] activatables;
    private bool currentActivation;
    public int abilityRequired;

    private void FixedUpdate()
    {
        // Using negate will deactivate with a specific powerup type instead of allowing with it.
        if (PlayerController.playerController.GetAbility() == abilityRequired)
        {
            activated = true;
        }
        else
        {
            activated = false;
        }
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
