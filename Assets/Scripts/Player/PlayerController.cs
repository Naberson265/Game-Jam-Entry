using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public enum PlayerAbility : int
{
    None = 0,
    Drone = 1,
    ConnectionLost = 100,
    OutlierReading = 200
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController { get; private set; }
    private Rigidbody rb;

    [Header("Objects")]
    public Transform camFixedDirTransform;
    private PhysicsMaterial playerPhysMat;
    private float playerPhysMatFriction;

    public GameObject leftOverBox;

    [Header("Movement")]
    public float moveSpeed = 8f;

    public float groundDrag = 2;
    public float airDrag = 0.4f;

    public float jumpHeight = 25f;
    public float launchMultiplier = 1.5f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    private bool readyToJump = true;

    public float coyoteTime = 0.15f;
    public float currentCoyoteTime;
    public float terminalVelocity = 50f;

    private bool canMove = true;
    private Vector3 movementDir = Vector3.zero;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Health and Ability")]
    // Abilities: 0-Default 1-Rocket/Dash 2-Feather/Lightweight 3-Metallic/Heavy 4-Explosive
    public List<int> health = new List<int> { 0, 0, 0, 0, 0 };
    public float[] healthToSize = { 0f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 4.75f, 5f };

    public float maxInvincibleTime = 1f;
    public float invincibleTime = 0f;

    public float abilityCooldown = 1f;
    public GameObject[] abilityModels;

    private bool usedAirAbility = false;

    [Header("Rocket Properties")]
    public float rocketSpeedMultiplier = 1.5f;
    public float dashForce = 40;
    private bool isDashing = false;

    [Header("Drone Properties")]
    public float floatTerminalVelocity = 0.2f;
    public float floatGravityPercentage = 0.5f;
    private bool isFloating = false;

    [Header("Metal Properties")]
    public float groundpoundForce = 50f;

    [Header("Spring Properties")]
    public float springLaunchMultiplier = 1.5f;

    void Start()
    {
        playerController = this;
        rb = GetComponent<Rigidbody>();
        playerPhysMat = gameObject.GetComponent<Collider>().material;
        playerPhysMatFriction = playerPhysMat.dynamicFriction;

        UpdateAppearance();
    }
    void Update()
    {
        // Grounded and Movement Direction
        grounded = Physics.BoxCast(gameObject.transform.position, gameObject.transform.localScale * 0.47f, Vector3.down, gameObject.transform.rotation, gameObject.transform.localScale.y * 0.05f, whatIsGround);
        movementDir = Input.GetAxisRaw("Vertical") * camFixedDirTransform.forward + Input.GetAxisRaw("Horizontal") * camFixedDirTransform.right;

        if (grounded)
        {
            usedAirAbility = false;
        }

        // Jumping
        if (Input.GetButton("Jump") && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Abilitying
        if (Input.GetButtonDown("Ability"))
        {
            Ability();
        }
        // Deactivation for click and hold abilities
        if (Input.GetButtonUp("Ability"))
        {
            DisableAbilities();
        }

        // Drag Changes in Air
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }

        // Invincibility Timer
        if (invincibleTime > 0f)
        {
            invincibleTime -= Time.deltaTime;
        }

        // Ability Cooldown
        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
        }
    }
	private void FixedUpdate()
	{
        if (canMove) {
            // on ground
            float speedMultiplier = 1;
            // Increase Speed while dashing
            if (isDashing)
            {
                speedMultiplier = rocketSpeedMultiplier;
            }
            if (grounded)
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * speedMultiplier, ForceMode.Force);
                playerPhysMat.dynamicFriction = playerPhysMatFriction;
                playerPhysMat.staticFriction = playerPhysMatFriction;
            }
            // in air
            else
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * airMultiplier * speedMultiplier, ForceMode.Force);
                playerPhysMat.dynamicFriction = 0;
                playerPhysMat.staticFriction = 0;
            }
            // Makes the player look in the direction they move.
            Vector3 directionToFace = transform.position + transform.forward + movementDir.normalized * 0.4f;
            transform.LookAt(directionToFace);
        }

        // We only want terminal velocity to effect downwards speed. When floating change this terminal velocity temporarily.
        float savedTerminalVel = terminalVelocity;
        if (isFloating)
        {
            terminalVelocity = floatTerminalVelocity;
            // Decrease Gravity
            rb.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y * rb.mass * floatGravityPercentage), 0), ForceMode.Force);
        }
        if (rb.linearVelocity.y < -terminalVelocity)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -terminalVelocity, rb.linearVelocity.z);
        }
        terminalVelocity = savedTerminalVel;

        // Check for Wall Clip, If so die.
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f, whatIsGround);
        foreach( Collider col in cols)
        {
            if(!col.isTrigger)
            {
                Damage(10, 3, true);
            }
        }
    }

    public void Ability()
    {
        // Currently most don't need the ability cooldown, so I left it up to each if statement
        // Rocket
        if (GetAbility() == 1)
        {
            if(grounded)
            {
                isDashing = true;
            } else if (!usedAirAbility)
            {
                usedAirAbility = true;
                // If the player is going the same direction as the dash, conserve that speed.
                float speed = Vector3.Dot(rb.linearVelocity, camFixedDirTransform.forward);
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(camFixedDirTransform.forward * (dashForce + speed), ForceMode.Impulse);
            }
        }
        // Drone
        else if (GetAbility() == 2)
        {
            isFloating = true;
        }
        // Metal
        else if (GetAbility() == 3 && !grounded && currentCoyoteTime <= 0f && !usedAirAbility)
        {
            rb.AddForce(Vector3.down * groundpoundForce, ForceMode.Impulse);
            invincibleTime = 5f;
            usedAirAbility = true;
        }
        // Spring
        else if (GetAbility() == 4 && abilityCooldown <= 0f && health.Count > 1)
        {
            Damage(1, 3, true);
        }
    }

    public void DisableAbilities()
    {
        isFloating = false;
        isDashing = false;
    }

    private void Jump(float mult = 1)
    {
        // Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Calculates force needed to get to jump height
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        rb.AddForce(transform.up * jumpVelocity * mult, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public int GetAbility()
    {
        if (health.Count > 0)
        {
            return health[health.Count - 1];
        } else
        {
            return 0;
        }
    }

    // Update Size and Model based on health
    private void UpdateAppearance()
    {
        float size = healthToSize[health.Count];
        gameObject.transform.localScale = new Vector3(size, size, size);

        // Update Model
        for (int i = 0; i < abilityModels.Length; i++) {
            if (i == GetAbility())
            {
                abilityModels[i].SetActive(true);
            } else
            {
                abilityModels[i].SetActive(false);
            }
        }
    }

    // Makes sure the Block should take damage, this is what everything else calls
    public void Damage(int damageAmount = 0, int damageLevel = 0, bool ignoreIFrames = false)
    {
        if ((invincibleTime <= 0f || ignoreIFrames) && health.Count > 0)
        {
            int deathAbility = GetAbility();
            if (damageLevel >= 3 ||
            damageLevel == 2 && !(GetAbility() == 3 && usedAirAbility) ||
            damageLevel == 1 && GetAbility() != 3 ||
            damageLevel == 0 && GetAbility() != 3)
            {
                for (int i = 0; i < damageAmount && health.Count > 0; i++)
                {
                    health.RemoveAt(health.Count - 1);
                }
                // Do damage animation.
                invincibleTime = maxInvincibleTime;
                StartCoroutine(DamageRoutine(deathAbility));
            }
        }
    }

    // Deal with damage animation and consequences here.
    private IEnumerator DamageRoutine(int ability)
    {
        print("Famage");
        GameObject droppedPart = Instantiate(leftOverBox, transform.position, transform.rotation);
        droppedPart.GetComponent<PlayerDupe>().SetModel(ability);
        droppedPart.transform.localScale = gameObject.transform.localScale;
        UpdateAppearance();

        //Conserve horizontal momentum when taking Damage
        Vector3 saveVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.isKinematic = true;
        canMove = false;
        yield return new WaitForSeconds(0.3f);
        rb.isKinematic = false;
        canMove = true;
        if (health.Count > 0)
        {
            DisableAbilities();

            float launchMult = launchMultiplier;
            if (ability == 4)
            {
                launchMult *= springLaunchMultiplier;
            }
            Jump(launchMult);
            rb.AddForce(saveVelocity,ForceMode.Impulse);
        }
        else
        {
            canMove = false;
            rb.isKinematic = true;
            yield return new WaitForSeconds(1f);
            GameController.ReloadLevel(); // Eventually set up Resettable
        }
        yield break;
    }

    public void Powerup(int ability)
    {
        health.Add(ability);
        UpdateAppearance();
    }
}
