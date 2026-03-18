using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class BombEnemy : Resettable
{
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float maxChaseDistance = 25f;
    public float moveAccel = 0f;
    public float accelSpeed = 1.5f;
    public float groundDrag = 2;
    public float airDrag = 0.4f;
    public float airMultiplier = 0.4f;
    public float terminalVelocity = 50f;
    public bool canMove = true;
    private Vector3 movementDir = Vector3.zero;
    public Vector3 target = Vector3.zero;
    public Animator modelAnimator;
    // Usually just the exclamation and brow.
    public GameObject[] chaseOnlyObjects;

    [Header("Audio")]
    public AudioSource bombAud;
    public AudioClip explodeSFX;

    [Header("Ground Check")]
    public bool grounded;
    public Vector3 spawnpoint;
    public LayerMask whatIsGround;
    // Typically the same but excluding pushable objects so that player dupes don't crush.
    public LayerMask whatCanCrush;

    [Header("Explosion")]
    public float willHurtTime = 0f;
    public float launchHeight = 40f;
    public bool hasExploded = false;

    void Start()
    {
        foreach (GameObject chObj in chaseOnlyObjects)
        {
            chObj.SetActive(false);
        }
        spawnpoint = transform.position;
        target = spawnpoint;
        rb = GetComponent<Rigidbody>();
        bombAud = GetComponent<AudioSource>();
        SaveDefault();
    }
    void Update()
    {
        // Grounded and Movement Direction
        grounded = Physics.BoxCast(gameObject.transform.position, gameObject.transform.localScale * 0.47f,
        Vector3.down, gameObject.transform.rotation, gameObject.transform.localScale.y * 0.05f, whatIsGround);
        // modelAnimator.SetBool("Grounded", grounded);
        // modelAnimator.SetBool("Moving", movementDir.magnitude > 0.2);
        willHurtTime -= Time.deltaTime;
        Vector3 playerAdjustedPos = new Vector3(PlayerController.playerController.transform.position.x,
        transform.position.y, PlayerController.playerController.transform.position.z);
        bool canChase = (transform.position - PlayerController.playerController.transform.position).magnitude <= maxChaseDistance;
        if (canMove)
        {
            if (moveAccel < 1f && canChase)
            {
                transform.LookAt(playerAdjustedPos);
                moveAccel += Time.deltaTime * accelSpeed;
                foreach (GameObject chObj in chaseOnlyObjects)
                {
                    chObj.SetActive(true);
                }
            }
            // Make the bomb move slowly in one direction after a chase.
            else if (!canChase && moveAccel > 0)
            {
                moveAccel -= Time.deltaTime * accelSpeed * 1.5f;
            }
            else if (canChase)
            {
                transform.LookAt(playerAdjustedPos);
                moveAccel = 1f;
            }
            else
            {
                moveAccel = 0f;
                foreach (GameObject chObj in chaseOnlyObjects)
                {
                    chObj.SetActive(false);
                }
            }
        }
        movementDir = moveAccel * transform.forward;
        // Drag Changes in Air
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }
	private void FixedUpdate()
	{
        if (canMove) {
            // on ground
            float speedMultiplier = 1;
            // Increase Speed while dashing
            if (grounded)
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * speedMultiplier, ForceMode.Force);
            }
            // in air
            else
            {
                rb.AddForce(movementDir.normalized * moveSpeed * 10f * airMultiplier * speedMultiplier, ForceMode.Force);
            }
        }

        // We only want terminal velocity to effect downwards speed.
        float savedTerminalVel = terminalVelocity;
        if (rb.linearVelocity.y < -terminalVelocity)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -terminalVelocity, rb.linearVelocity.z);
        }
        terminalVelocity = savedTerminalVel;
        // Check for Wall Clip, If so explode.
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f, whatCanCrush);
        foreach( Collider col in cols)
        {
            if(!col.isTrigger)
            {
                Explode(1, 0);
            }
        }
    }

    public IEnumerator Explode(float mult = 1, float waitTime = 0.7f)
    {
        float explodeVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * launchHeight * mult);
        canMove = false;
        rb.isKinematic = true;
        bombAud.PlayOneShot(explodeSFX);
        // modelAnimator.Play("Explode");
        yield return new WaitForSeconds(waitTime);
        rb.isKinematic = false;
        rb.AddForce(transform.up * explodeVelocity, ForceMode.Impulse);
        if (willHurtTime > 0f)
        {
            PlayerController.playerController.Damage(1, 3, false);
        }
        yield break;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if (canMove)
            {
                Explode();
            }
            willHurtTime = 0.1f;
        }
    }
    protected override void ResetObject()
    {
        hasExploded = false;
        transform.position = spawnpoint;
    }
    protected override void SaveDefault()
    {
        
    }
}
