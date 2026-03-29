using System;
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
    public Rigidbody rb;

    [Header("Objects")]
    public GameObject mainCam;
    public Animator modelAnimator;
    public Transform camFixedDirTransform;
    public GameObject leftOverBox;
    public GameObject face;

    private bool cheat1;
    private bool cheat2;
    private DitherTransition ditherer;

    [Header("Face")]
    private float blinkCooldown = 4;
    // Check the mouth spritesheet for the order of faces (left to right).
    public int mouthState = 0;
    // 0-Default (only one that blinks) 1-Panicked 2-Shut Eyes
    public int eyeState = 0;

    [Header("Movement")]
    public float moveSpeed = 8f;

    public float groundDrag = 2;
    public float airDrag = 0.4f;

    public float jumpHeight = 25f;
    public float minJumpHeight = 15f;
    private bool jumping = false;
    private bool launching = false;
    // To make jumping run in FixedUpdate and not Update.
    private bool aboutToJump = false;
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
    public AudioSource playerAudio;
    public AudioSource playerLoopingAudio;
    public AudioClip jumpSFX;
    public AudioClip droneSFX;
    public AudioClip hitSFX;
    public AudioClip rocketSFX;
    public AudioClip springSFX;
    public AudioClip speedSFX;
    public AudioClip powerupSFX;
    public AudioClip spikeBreakSFX;
    public AudioClip poundSFX;
    public AudioClip landSFX;
    
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

    public bool usedAirAbility = false;

    [Header("Rocket Properties")]
    public float rocketSpeedMultiplier = 1.5f;
    public float dashForce = 40;
    private bool isDashing = false;
    public ParticleSystem runParticle1;
    public ParticleSystem runParticle2;
    public ParticleSystem dashParticle;

    [Header("Drone Properties")]
    public float floatTerminalVelocity = 0.2f;
    public float floatGravityPercentage = 0.5f;
    private bool isFloating = false;

    [Header("Metal Properties")]
    public float groundPoundForce = 50f;
    public float groundPoundHeight = 2f;
    public float groundPoundUpTime = 0.15f;
    public float groundPoundPause = 0.15f;
    public ParticleSystem groundPoundParticle;
    public bool slamingDowm = false;

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
        mainCam = Camera.main.gameObject;
        ditherer = FindFirstObjectByType<DitherTransition>();
        UpdateAppearance();
        SaveDefault();

        blinkCooldown = UnityEngine.Random.Range(2f, 7f);
    }

    IEnumerator waitForBool()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        slamingDowm = false;
    }
    void Update()
    {
        // Grounded and Movement Direction
        movementDir = Input.GetAxisRaw("Vertical") * camFixedDirTransform.forward + Input.GetAxisRaw("Horizontal") * camFixedDirTransform.right;
   
        // Jumping
        if (Input.GetButton("Jump") && readyToJump && (grounded || currentCoyoteTime > 0f) && canMove)
        {
            aboutToJump = true;
        }
        // Boolean to track if player is holding down jump
        if (Input.GetButton("Jump") && canMove)
        {
            jumping = true;
        } else
        {
            jumping = false;
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
        // Restart from last Checkpoint
        if (Input.GetKeyDown(KeyCode.R))
        {
            Damage(10, 3, true);
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
        // Cheats
        if (Input.GetKey(KeyCode.O))
        {
            cheat1 = true;
        } else
        {
            cheat1 = false;
        }
        if (Input.GetKey(KeyCode.L))
        {
            cheat2 = true;
        }
        else
        {
            cheat2 = false;
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
            face.SetActive(false);
        }
        else
        {
            face.SetActive(true);
        }

        // Blink
        if (eyeState == 1)
        {
            // If panicked, the blink time decreases faster.
            blinkCooldown -= Time.deltaTime * 2;
        }
        else blinkCooldown -= Time.deltaTime;
        if (blinkCooldown < 0)
        {
            if (eyeState == 0)
            {
                modelAnimator.SetTrigger("Blink");
            }
            blinkCooldown = UnityEngine.Random.Range(2f, 7f);
        }
    }
    private void FixedUpdate()
    {
        grounded = Physics.BoxCast(gameObject.transform.position, gameObject.transform.localScale * 0.47f, Vector3.down, gameObject.transform.rotation, gameObject.transform.localScale.y * 0.05f, whatIsGround);
        if (grounded)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            currentCoyoteTime = coyoteTime;
            usedAirAbility = false;
            if (slamingDowm) StartCoroutine(waitForBool());
        }
        else if (currentCoyoteTime > 0f)
        {
            currentCoyoteTime -= Time.fixedDeltaTime;
        }

        modelAnimator.SetBool("Grounded", grounded);
        modelAnimator.SetBool("Moving", movementDir.magnitude > 0.2f && canMove);
        if(cheat1)
        {
            rb.AddForce(new Vector3(0, 100, 0));
        }

        if (cheat2)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        } else
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        if (canMove)
        {
            // on ground
            if (isDashing)
            {
                rb.AddForce(gameObject.transform.forward * moveSpeed * 10f * (rocketSpeedMultiplier - 1), ForceMode.Force);
            }
            if (grounded)
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f, ForceMode.Force);
                launching = false;
            }
            // in air
            else
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

                // Counteract drag on the y axis so gravity is dragless.
                rb.AddForce((rb.linearDamping * rb.linearVelocity.y) * Vector3.up, ForceMode.Force);
            }
            if (aboutToJump)
            {
                Jump();
            }
            // Makes the player look in the direction they move.
            Vector3 directionToFace = transform.position + transform.forward + movementDir.normalized * 0.4f;
            transform.LookAt(directionToFace);
            // Cancel out the above if Turn With Camera is enabled.
            if (PlayerPrefs.GetInt("TurnWithCamera") == 2) transform.rotation = camFixedDirTransform.rotation;
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

        // If player is holding down jump, counteract gravity so the jump height reaches jumpHeight instead of minJumpHeight
        if ((jumping || launching) && rb.linearVelocity.y > 0)
        {
            rb.AddForce(transform.up * (Physics.gravity.y * (minJumpHeight - jumpHeight) / jumpHeight), ForceMode.Force);
        }

        // Check for Wall Clip, If so die.
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f, whatCanCrush);
        foreach (Collider col in cols)
        {
            if (!col.isTrigger && !cheat2)
            {
                Damage(10, 3, false);
            }
        }
        // If fallen off map, die
        if (transform.position.y < -500)
        {
            Damage(10, 3, false);
        }
    }

    public void Ability()
    {
        // Currently most don't need the ability cooldown, so I left it up to each if statement
        // Rocket
        if (GetAbility() == 1)
        {
            if (grounded)
            {
                runParticle1.Play();
                runParticle2.Play();
                playerLoopingAudio.PlayOneShot(rocketSFX);
                isDashing = true;
            }
            else if (!usedAirAbility)
            {
                dashParticle.Play();
                playerAudio.PlayOneShot(speedSFX);
                usedAirAbility = true;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(gameObject.transform.forward * dashForce, ForceMode.Impulse);
            }
        }
        // Drone
        else if (GetAbility() == 2)
        {
            playerLoopingAudio.PlayOneShot(droneSFX);
            isFloating = true;
            modelAnimator.SetBool("Floating", true);
        }
        // Metal
        else if (GetAbility() == 3 && !grounded && currentCoyoteTime <= 0f && !usedAirAbility)
        {
            StartCoroutine(GroundPound());
            invincibleTime = 5f;
            usedAirAbility = true;
        }
        // Spring
        else if (GetAbility() == 4 && abilityCooldown <= 0f && health.Count > 1)
        {
            Damage(1, 3, true);
        }
    }
    private IEnumerator GroundPound()
    {
        slamingDowm = true;
        playerAudio.PlayOneShot(speedSFX);
        canMove = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * groundPoundHeight;

        float time = 0f;
        
        while (time < groundPoundUpTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / groundPoundUpTime);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        // pause
        yield return new WaitForSeconds(groundPoundPause);

        // slam down
        modelAnimator.SetTrigger("GroundPound");
        rb.useGravity = true;
        rb.linearVelocity = new Vector3(0f, -groundPoundForce, 0f);
        playerAudio.PlayOneShot(jumpSFX);

        while (rb.linearVelocity.y < -2)
        {
            yield return new WaitForSeconds(0.02f);
        }
        playerAudio.PlayOneShot(poundSFX);
        groundPoundParticle.Play();
        canMove = true;
    }
    public void DisableAbilities()
    {
        playerLoopingAudio.Stop();
        isFloating = false;
        isDashing = false;
        runParticle1.Stop();
        runParticle2.Stop();
        modelAnimator.SetBool("Floating", false);
    }
    private void Jump(float mult = 1, bool playJumpAnim = true)
    {
        aboutToJump = false;
        if (playJumpAnim)
        {
            modelAnimator.SetTrigger("Jump");
            playerAudio.PlayOneShot(jumpSFX);
        }
        // Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        // Calculates force needed to get to jump height
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * minJumpHeight * mult);
        rb.AddForce(transform.up * jumpVelocity, ForceMode.Impulse);

        readyToJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
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
        }
        else
        {
            return 0;
        }
    }

    // Update Size and Model based on health
    public void UpdateAppearance()
    {
        float size = healthToSize[health.Count];
        gameObject.transform.localScale = new Vector3(size, size, size);

        // Update Model
        for (int i = 0; i < abilityModels.Length; i++)
        {
            if (i == GetAbility())
            {
                abilityModels[i].SetActive(true);
            }
            else
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
        playerAudio.PlayOneShot(hitSFX);
        UpdateAppearance();

        //Conserve horizontal momentum when taking Damage
        Vector3 saveVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        canMove = false;
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        rb.isKinematic = false;
        DisableAbilities();
        if (health.Count > 0)
        {
            modelAnimator.SetTrigger("Damage");
            float launchMult = launchMultiplier;
            if (ability == 4)
            {
                launchMult *= springLaunchMultiplier;
                playerAudio.PlayOneShot(springSFX);
            }
            Jump(launchMult, false);
            launching = true;
            usedAirAbility = false;
            rb.AddForce(saveVelocity, ForceMode.Impulse);
        }
        else
        {
            canMove = false;
            rb.isKinematic = true;
            ditherer.StartAnim("Start");
            yield return new WaitForSeconds(1f);
            // Reset all Resettables unless the bool in GC states otherwise.
            if (GameController.gameController.reloadOnDeath)
            {
                GameController.ReloadLevel();
            }
            else
            {
                ditherer.StartAnim("End");
                Resettable.ResetAll();
            }
        }
        yield break;
    }

    private IEnumerator RespawnRoutine()
    {
        for (int i = 0; i < 100; i++)
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
        DisableAbilities();
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
