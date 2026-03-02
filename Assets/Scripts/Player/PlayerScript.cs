using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (invincibleTime <= 0f && health <= 0) gameController.ReloadLevel();
    }
	public void Movement()
	{
		movement = Vector3.zero;
		sideMovement = Vector3.zero;
        movement += Input.GetAxis("Vertical") * camFixedDirTransform.forward;
        sideMovement += Input.GetAxis("Horizontal") * camFixedDirTransform.right;
        // Acceleration.
        float newAccelSpeed = accelSpeed * Time.deltaTime;
        if (ability == 1f) accelSpeed *= 1.25f;
        if (Input.GetAxis("Vertical") > 0.01f || Input.GetAxis("Horizontal") > 0.01f || Input.GetAxis("Vertical") < -0.01f || Input.GetAxis("Horizontal") < -0.01f)
        {
            if (playerSpeed < speedBenchmark[0]) playerSpeed += newAccelSpeed;
            else if (playerSpeed < speedBenchmark[1] && charControl.isGrounded) playerSpeed += newAccelSpeed / 2.5f;
            else if (playerSpeed < speedBenchmark[2] && charControl.isGrounded) playerSpeed += newAccelSpeed / 5f;
            else if (playerSpeed >= speedBenchmark[2]) playerSpeed = speedBenchmark[2];
        }
        else playerSpeed = 0f;
        newAccelSpeed /= Time.deltaTime;
        Vector3 combinedMovement = (movement + sideMovement);
        if (combinedMovement.sqrMagnitude > 1f) combinedMovement.Normalize();
        // Makes the player look in the direction they move.
        Vector3 directionToFace = transform.position + combinedMovement;
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
            yVelocity = 0f;
            currentCoyoteTime = coyoteTime;
        }
        if (Input.GetButton("Jump") && currentCoyoteTime > 0f)
        {
            yVelocity += jumpHeight;
            currentCoyoteTime = 0f;
        }
        // Makes sure the player's y velocity never goes past the terminal velocity.
        if (yVelocity > terminalVelocity) yVelocity = terminalVelocity;
        if (yVelocity < -terminalVelocity) yVelocity = -terminalVelocity;
		playerSpeed *= Time.deltaTime;
		moveDirection = combinedMovement * playerSpeed;
        moveDirection += new Vector3 (0f, yVelocity * Time.deltaTime, 0f);
		charControl.Move(moveDirection);
		playerSpeed /= Time.deltaTime; // Converts the speed back to normal numbers.
    }
    public void Damage(float newIFrames, int damageDealt, bool ignoreIFrames)
    {
        // Ignore IFrames makes the player take damage even when they are on damage cooldown.
        if (invincibleTime <= 0f || ignoreIFrames)
        {
            invincibleTime += newIFrames;
            GameObject droppedPart = Instantiate(leftOverBox, transform.position, transform.rotation);
            droppedPart.transform.localScale = playerModel.transform.localScale;
            droppedPart.GetComponent<PlayerDupe>().ps = this;
            health -= damageDealt;
            if (health > 0)
            {
                playerModel.transform.localScale = new Vector3(healthToSize[health], healthToSize[health], healthToSize[health]);
                charControl.radius = (healthToSize[health] / 2) - 0.01f;
                yVelocity += jumpHeight;
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

    public int ability;
    // Abilities: 0-Default 1-Rocket/Dash 2-Feather/Lightweight 3-Metallic/Heavy 4-Explosive
    public float playerSpeed;
    public float[] speedBenchmark = {10f, 20f, 35f};
    public float[] healthToSize = {0f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 4.75f, 5f};
    public float accelSpeed = 20f;
    public float jumpHeight = 25f;
    public float yVelocity = 0f;
    public float terminalVelocity = 50f;
    public float gravity = 60f;
    public float invincibleTime = 0f;
    public float coyoteTime = 0.15f;
    public float currentCoyoteTime;
    public int health = 5;
    public bool canMove = true;
	public Vector3 movement = Vector3.zero;
	public Vector3 sideMovement = Vector3.zero;
	public Vector3 moveDirection;
    public Collider charCollider;
    public Transform camTransform;
	public Transform camFixedDirTransform;
    public GameObject playerModel;
    public GameObject leftOverBox;
    public CharacterController charControl;
    public GameController gameController;
}
