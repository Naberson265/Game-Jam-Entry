using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public enum PlayerAbility : int
{
    None = 0,
    Drone = 1,
    ConnectionLost = 100,
    OutlierReading = 200
}

public class PlayerController : Resettable
{
    public static PlayerController playerController { get; private set; }
    private Rigidbody rb;

    [Header("Objects")]
    public GameObject mainCam;
    public Transform camFixedDirTransform;
    public Animator modelAnimator;
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
    public bool canMove = true;
    private Vector3 movementDir = Vector3.zero;

    [Header("Audio")]
    private AudioSource playerAudio;
    public AudioClip jumpSFX;
    public AudioClip droneSFX;
    public AudioClip hitSFX;
    public AudioClip rocketSFX;
    public AudioClip springSFX;
    public AudioClip speedSFX;
    public AudioClip powerupSFX;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    // Typically the same but excluding pushable objects so that player dupes don't kill.
    public LayerMask whatCanCrush;
    public bool grounded;

    [Header("Health and Ability")]
    // Abilities: 0-Default 1-Rocket/Dash 2-Feather/Drone 3-Metallic/Heavy 4-Spring
    public List<int> health = new List<int> { 0, 0, 0, 0, 0 };
    public float[] healthToSize = { 0f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 4.75f, 5f };

    public float maxInvincibleTime = 1f;
    public float invincibleTime = 0f;

    public float abilityCooldown = 1f;
    public GameObject[] abilityModels;
    public GameObject pModelParent;

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


    // Resettable Defaults
    private List<int> savedHealth;
    public Vector3 spawnpoint;

    void Start()
    {
        spawnpoint = transform.position;
        playerController = this;
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        pModelParent = abilityModels[0].transform.parent.gameObject;
        mainCam = Camera.main.gameObject;
        UpdateAppearance();
        SaveDefault();
    }
    void Update()
    {
        // Grounded and Movement Direction
        grounded = Physics.BoxCast(gameObject.transform.position, gameObject.transform.localScale * 0.47f, Vector3.down, gameObject.transform.rotation, gameObject.transform.localScale.y * 0.05f, whatIsGround);
        movementDir = Input.GetAxisRaw("Vertical") * camFixedDirTransform.forward + Input.GetAxisRaw("Horizontal") * camFixedDirTransform.right;
        modelAnimator.SetBool("Grounded", grounded);
        modelAnimator.SetBool("Moving", movementDir.magnitude > 0.2);
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
        // Cheats
        if (Input.GetKey(KeyCode.O))
        {
            rb.AddForce(new Vector3(0,20,0));
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
        // If the camera is very close to or inside the player model, disable it.
        if ((transform.position - mainCam.transform.position).magnitude < healthToSize[health.Count])
        {
            pModelParent.SetActive(false);
        }
        else
        {
            pModelParent.SetActive(true);
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
            }
            // in air
            else
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * airMultiplier * speedMultiplier, ForceMode.Force);
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
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f, whatCanCrush);
        foreach( Collider col in cols)
        {
            if(!col.isTrigger)
            {
                Damage(10, 3, true);
                print("Crushed!");
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
                rb.AddForce(gameObject.transform.forward * dashForce, ForceMode.Impulse);
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
            playerAudio.PlayOneShot(springSFX);
        }
    }

    public void DisableAbilities()
    {
        isFloating = false;
        isDashing = false;
    }

    private void Jump(float mult = 1, bool playJumpAnim = true)
    {
        if (playJumpAnim)
        {
            modelAnimator.Play("Jump");
            playerAudio.PlayOneShot(jumpSFX);
        }
        // Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        // Calculates force needed to get to jump height
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight * mult);
        rb.AddForce(transform.up * jumpVelocity, ForceMode.Impulse);
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
        GameObject droppedPart = Instantiate(leftOverBox, transform.position, transform.rotation);
        droppedPart.GetComponent<PlayerDupe>().SetModel(ability);
        droppedPart.transform.localScale = gameObject.transform.localScale;
        if (!playerAudio.isPlaying) playerAudio.PlayOneShot(hitSFX);
        UpdateAppearance();

        //Conserve horizontal momentum when taking Damage
        Vector3 saveVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        canMove = false;
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.3f);
        canMove = true;
        rb.isKinematic = false;
        if (health.Count > 0)
        {
            DisableAbilities();
            modelAnimator.Play("Damage");
            float launchMult = launchMultiplier;
            if (ability == 4)
            {
                launchMult *= springLaunchMultiplier;
            }
            Jump(launchMult, false);
            rb.AddForce(saveVelocity,ForceMode.Impulse);
        }
        else
        {
            canMove = false;
            rb.isKinematic = true;
            yield return new WaitForSeconds(1f);
            Resettable.ResetAll(); // Eventually set up Resettable
        }
        yield break;
    }

    private IEnumerator RespawnRoutine()
    { 
        for(int i = 0; i < 100; i++)
        {
            float size = healthToSize[health.Count];
            Vector3 goalScale = new Vector3(size, size, size);
            transform.localScale = Vector3.Lerp(transform.localScale, goalScale, 0.1f);
            yield return new WaitForSeconds(0.005f);
        }
        UpdateAppearance();
        canMove = true;
        rb.isKinematic = false;
    }

    public void Powerup(int ability)
    {
        health.Add(ability);
        playerAudio.PlayOneShot(powerupSFX);
        UpdateAppearance();
    }

    protected override void ResetObject()
    {
        health = new List<int>(savedHealth);
        transform.position = spawnpoint;
        StartCoroutine(RespawnRoutine());
    }

    protected override void SaveDefault()
    {
        savedHealth = new List<int>(health);
        // Spawnpoint is set by checkpoints because I'm lazy
    }
}
