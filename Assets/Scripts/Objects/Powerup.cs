using UnityEngine;

public class Powerup : Resettable
{
    private bool canPickup = true;
    private int respawnHealth = -1;
    private float respawnTime = 0;
    public bool canRespawn = false;
    public float TimeBeforeRespawn = 1;
    [SerializeField] private int powerupType = 0;
    [SerializeField] private SpriteRenderer iconRenderer;

    private void Start()
    {
        iconRenderer.sprite = IconManager.iconManager.powerUpIcons[powerupType];
    }
    private void Update()
    {
        if (canRespawn && !canPickup && PlayerController.playerController.health.Count <= respawnHealth)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime > TimeBeforeRespawn)
            {
                respawnTime = 0;
                canPickup = true;
                iconRenderer.enabled = true;
                respawnHealth = -1;
            }
        }
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
            respawnHealth = PlayerController.playerController.health.Count;
            PlayerController.playerController.Powerup(powerupType);
        }
    }
}
