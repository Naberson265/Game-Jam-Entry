using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour
{
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        charCollider = GetComponent<Collider>();
        playerModel.transform.localScale = new Vector3(healthToSize[health], healthToSize[health], healthToSize[health]);
        charControl.radius = (healthToSize[health] / 2) - 0.01f;
    }
    void Update()
    {
        if (canMove) Movement();
        if (invincibleTime > 0f) invincibleTime -= Time.deltaTime;
        if (invincibleTime <= 0f && health <= 0) GameController.ReloadLevel();
        powerQueueDisplays[0].sprite = powerUpIcons[ability];
        if (Input.GetButtonDown("Ability")) Ability();
        if (abilityCooldown > 0f)
        {
            powerQueueDisplays[0].color = Color.grey;
            // Cooldown will only decrease sometimes so abilities can't be spammed (especially mid-air ones).
            if (ability == 0 || ability == 4) abilityCooldown -= Time.deltaTime;
            else if (charControl.isGrounded && ability == 2) abilityCooldown -= Time.deltaTime;
            else if (charControl.isGrounded && ability == 3) abilityCooldown -= Time.deltaTime;
        }
        else powerQueueDisplays[0].color = Color.white;
    }
	public void Movement()
	{
		movement = Vector3.zero;
		sideMovement = Vector3.zero;
        movement += Input.GetAxis("Vertical") * camFixedDirTransform.forward;
        sideMovement += Input.GetAxis("Horizontal") * camFixedDirTransform.right;
        // Acceleration.
        float newAccelSpeed = accelSpeed * Time.deltaTime; // Function specific to prevent issues with timescale/pauses.
        float newPlayerSpeed = playerSpeed; // Function specific to prevent issues with timescale/pauses.
        if (ability == 1f) newAccelSpeed *= 1.25f;
        if (Input.GetAxis("Vertical") > 0.01f || Input.GetAxis("Horizontal") > 0.01f || Input.GetAxis("Vertical") < -0.01f || Input.GetAxis("Horizontal") < -0.01f)
        {
            if (newPlayerSpeed < speedBenchmark[0]) newPlayerSpeed += newAccelSpeed;
            else if (newPlayerSpeed < speedBenchmark[1] && charControl.isGrounded) newPlayerSpeed += newAccelSpeed / 2.5f;
            else if (newPlayerSpeed < speedBenchmark[2] && charControl.isGrounded) newPlayerSpeed += newAccelSpeed / 5f;
            else if (newPlayerSpeed >= speedBenchmark[2]) newPlayerSpeed = speedBenchmark[2];
        }
        else newPlayerSpeed = 0f;
        newAccelSpeed /= Time.deltaTime;
        Vector3 combinedMovement = (movement + sideMovement);
        if (combinedMovement.sqrMagnitude > 1f) combinedMovement.Normalize();
        // Makes the player look in the direction they move.
        lookAtDir = Input.GetAxisRaw("Vertical") * camFixedDirTransform.forward + Input.GetAxisRaw("Horizontal") * camFixedDirTransform.right;
        Vector3 directionToFace = transform.position + transform.forward + lookAtDir.normalized * 0.4f;
        transform.LookAt(directionToFace);
        // Jumping and gravity.
        if (!charControl.isGrounded)
        {
            float powerGravityEffect = 1f;
            if (ability == 2) powerGravityEffect = 0.75f;
            else if (ability == 3) powerGravityEffect = 1.3f;
            yVelocity -= gravity * Time.deltaTime * powerGravityEffect;
            if (currentCoyoteTime > 0f) currentCoyoteTime -= Time.deltaTime;
        }
        else
        {
            if (ability != 3 || (ability == 3 && abilityCooldown <= 0f)) yVelocity = 0f;
            else if (abilityCooldown > 0f)
            {
                yVelocity = jumpHeight * 1.3f;
                abilityCooldown = 0f;
            }
            currentCoyoteTime = coyoteTime;
        }
        if (Input.GetButton("Jump") && currentCoyoteTime > 0f)
        {
            yVelocity += jumpHeight;
            currentCoyoteTime = 0f;
        }
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.up, out raycastHit, (playerModel.transform.localScale.x / 2f), rayLayerMask, QueryTriggerInteraction.Ignore) && yVelocity > 0.1f)
        {
            // Makes sure that when the player collides with something from above they don't stick to it and instead fall down.
            yVelocity = 0f;
        }
        // Makes sure the player's y velocity never goes past the terminal velocity.
        if (yVelocity > terminalVelocity) yVelocity = terminalVelocity;
        if (yVelocity < -terminalVelocity) yVelocity = -terminalVelocity;
		newPlayerSpeed *= Time.deltaTime;
		moveDirection = combinedMovement * newPlayerSpeed;
        moveDirection += new Vector3 (0f, yVelocity * Time.deltaTime, 0f);
		charControl.Move(moveDirection);
		newPlayerSpeed /= Time.deltaTime; // Converts the speed back to normal numbers.
        playerSpeed = newPlayerSpeed; // Makes the real playerSpeed variable equal to the function specific one.
    }
    public void Damage(int damageDealt, bool ignoreIFrames, int damageLevel)
    {
        // IgnoreIFrames makes the player take damage even when they are on damage cooldown.
        // DamageDealt controls the amount of damage the player takes.
        // DamageLevel determines whether or not the attack can be tanked.
        // 0 can be tanked by Metallic and high speed
        // 1 can be tanked by Metallic only
        // 2 can only be tanked during a Metallic ground pound
        // >3 is forced
        if (invincibleTime <= 0f || ignoreIFrames)
        {
            invincibleTime = maxInvincibleTime;
            if (damageLevel >= 3 ||
            damageLevel == 2 && !(ability == 3 && abilityCooldown <= 0f) ||
            damageLevel == 1 && ability != 3 ||
            damageLevel == 0 && ability != 3 && playerSpeed <= 30f)
            {
                GameObject droppedPart = Instantiate(leftOverBox, transform.position, transform.rotation);
                droppedPart.transform.localScale = playerModel.transform.localScale;
                health -= damageDealt;
                ability = 0;
                if (health > 0)
                {
                    playerModel.transform.localScale = new Vector3(healthToSize[health], healthToSize[health], healthToSize[health]);
                    charControl.radius = (healthToSize[health] / 2) - 0.01f;
                    yVelocity += jumpHeight * 2f;
                    charControl.Move(new Vector3(0f, yVelocity * Time.deltaTime, 0f));
                    playerSpeed = 0f;
                }
                else
                {
                    health = 0;
                    canMove = false;
                    playerModel.transform.localScale = new Vector3(0f, 0f, 0f);
                    charControl.enabled = false;
                    invincibleTime = 2f;
                }
            }
        }
    }
    public void Ability()
    {
        if (abilityCooldown <= 0f)
        {
            if (!charControl.isGrounded && currentCoyoteTime <= 0f && ability == 2 && yVelocity < jumpHeight / 1.5f)
            {
                yVelocity = jumpHeight / 1.5f;
                currentCoyoteTime = 0f;
                abilityCooldown = 0.05f;
            }
            if (!charControl.isGrounded && currentCoyoteTime <= 0f && ability == 3)
            {
                yVelocity = -terminalVelocity;
                invincibleTime = 5f;
                abilityCooldown = 0.05f;
            }
            if (ability == 4 && abilityCooldown <= 0f && health > 1)
            {
                Damage(1, true, 3);
            }
        }
    }

    [Header("Health and Abilities")]
    public float[] healthToSize = {0f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 4.75f, 5f};
    public float maxInvincibleTime = 2f;
    public float invincibleTime = 0f;
    public int health = 5;
    public Image[] powerQueueDisplays;
    public Sprite[] powerUpIcons;
    public float abilityCooldown = 1f;
    public int ability;
    // Abilities: 0-Default 1-Rocket/Dash 2-Feather/Lightweight 3-Metallic/Heavy 4-Explosive

    [Header("Movement")]
    public float accelSpeed = 32f;
    public float jumpHeight = 25f;
    public float yVelocity = 0f;
    public float terminalVelocity = 60f;
    public float gravity = 60f;
    public float coyoteTime = 0.15f;
    public float currentCoyoteTime;
    public float playerSpeed;
    public float[] speedBenchmark = {10f, 20f, 35f};
    public bool canMove = true;
	private Vector3 movement = Vector3.zero;
	private Vector3 sideMovement = Vector3.zero;
	private Vector3 moveDirection;
    private Vector3 lookAtDir = Vector3.zero;
    public Collider charCollider;
	public LayerMask rayLayerMask;
    
    [Header("Objects")]
    public Transform camTransform;
	public Transform camFixedDirTransform;
    public GameObject playerModel;
    public GameObject leftOverBox;
    public CharacterController charControl;
}
