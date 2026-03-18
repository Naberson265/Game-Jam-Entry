using UnityEngine;
using UnityEngine.UI;

public class PowerUpSpecificDoor : Activatable
{
    [SerializeField] private bool negated = false;
    [SerializeField] private Activatable[] activatables;
    [SerializeField] private Sprite[] powerIcons;
    private bool currentActivation;
    public GameObject[] deniedCrosses;
    public SpriteRenderer[] iconImages;
    public int abilityRequired;
    void Start()
    {
        if (negated)
        {
            foreach (GameObject dCross in deniedCrosses)
            {
                dCross.SetActive(true);
            }
        }
        foreach (SpriteRenderer iconImg in iconImages)
        {
            iconImg.sprite = powerIcons[abilityRequired];
        }
    }
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
