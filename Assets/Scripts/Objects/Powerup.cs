using UnityEngine;

public class Powerup : Resettable
{
    public PlayerController ps;
    private bool canPickup = true;
    private float respawnTime;
    public bool canRespawn = false;
    public float timeBetweenRespawns = 15f;
    [SerializeField] private int powerupType = 0;
    [SerializeField] private SpriteRenderer iconRenderer;

    private void Start()
    {
        ps = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        iconRenderer.sprite = IconManager.iconManager.powerUpIcons[powerupType];
        respawnTime = timeBetweenRespawns;
    }
    private void Update()
    {
        if (ps.GetAbility() != powerupType)
        {
            if (respawnTime > 0f && !canPickup && canRespawn) respawnTime -= Time.deltaTime;
            else
            {
                canPickup = true;
                iconRenderer.enabled = true;
                respawnTime = timeBetweenRespawns;
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
            ps.Powerup(powerupType);
        }
    }
}
