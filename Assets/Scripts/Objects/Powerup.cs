using UnityEngine;

public class Powerup : Resettable
{
    private bool canPickup = true;
    [SerializeField] private int powerupType = 0;
    [SerializeField] private SpriteRenderer iconRenderer;

    private void Start()
    {
        iconRenderer.sprite = IconManager.iconManager.powerupIcons[powerupType];
    }

    // What everything should reset to.
    private bool defaultState = true;

    protected override void ResetDefault()
    {
        defaultState = true;
    }

    protected override void ResetObject()
    {
        canPickup = defaultState;
        iconRenderer.enabled = defaultState;
    }

    protected override void SaveDefault()
    {
        defaultState = canPickup;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && canPickup)
        {
            canPickup = false;
            iconRenderer.enabled = false;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.Powerup(powerupType);
        }
    }
}
