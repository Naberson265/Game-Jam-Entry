using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerAbility : int
{
    None = 0,
    Drone = 1,
    ConnectionLost = 100,
    OutlierReading = 200
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 7f;

    public float groundDrag = 5;

    public float jumpHeight = 25f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    private bool readyToJump = true;

    public float coyoteTime = 0.15f;
    public float currentCoyoteTime;
    public float terminalVelocity = 50f;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Health and Ability")]
    // Abilities: 0-Default 1-Rocket/Dash 2-Feather/Lightweight 3-Metallic/Heavy 4-Explosive
    public int health = 5;
    public float[] healthToSize = { 0f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 4.75f, 5f };

    public float maxInvincibleTime = 2f;
    public float invincibleTime = 0f;

    public int ability;
    public Image[] powerQueueDisplays;
    public Sprite[] powerUpIcons;


    [Header("Objects")]
    public Transform camFixedDirTransform;
    public GameObject playerModel;
    public GameObject leftOverBox;

    private bool canMove = true;
    private Vector3 movementDir = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerModel.transform.localScale = new Vector3(healthToSize[health], healthToSize[health], healthToSize[health]);
    }
    void Update()
    {
        // Grounded and Movement Direction
        // TODO: CHANGE THIS RAYCAST TO A BoxCast.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerModel.transform.localScale.y * 0.5f + 0.3f, whatIsGround);
        movementDir = Input.GetAxisRaw("Vertical") * camFixedDirTransform.forward + Input.GetAxisRaw("Horizontal") * camFixedDirTransform.right;
        // When to Jump
        if (Input.GetButton("Jump") && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        if (invincibleTime > 0f) invincibleTime -= Time.deltaTime;
        powerQueueDisplays[0].sprite = powerUpIcons[ability];
    }
	private void FixedUpdate()
	{
        if (canMove) {
            // on ground
            if (grounded)
                rb.AddForce(movementDir.normalized * moveSpeed * 10f, ForceMode.Force);

            // in air
            else if (!grounded)
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        if (rb.linearVelocity.y > terminalVelocity) rb.linearVelocity = new Vector3(rb.linearVelocity.x, terminalVelocity, rb.linearVelocity.z);
        if (rb.linearVelocity.y < -terminalVelocity) rb.linearVelocity = new Vector3(rb.linearVelocity.x, -terminalVelocity, rb.linearVelocity.z);
        // Makes the player look in the direction they move.
        Vector3 directionToFace = transform.position + transform.forward + movementDir.normalized * 0.4f;
        transform.LookAt(directionToFace);
    }

    private void Jump()
    {
        // Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Calculates force needed to get to jump height
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        rb.AddForce(transform.up * jumpVelocity, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    // Turn Damage into an IEnumerator
    public IEnumerator Damage(int damageDealt, bool ignoreIFrames)
    {
        invincibleTime = maxInvincibleTime;
        yield return new WaitForSeconds(1);
        if (health <= 0) GameController.ReloadLevel();
        yield break;
    }
}
