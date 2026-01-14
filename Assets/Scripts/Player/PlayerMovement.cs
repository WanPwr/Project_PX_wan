using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    // --- NEW DIALOGUE SETTING ---
    [HideInInspector] public bool isLockedByDialogue = false;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float acceleration = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public int maxJumps = 2;
    private int jumpCount = 0;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Landing Threshold")]
    public float landingThreshold = 0.1f;

    [Header("Attack Settings")]
    public KeyCode attackKey = KeyCode.J;
    public GameObject attackHitbox;

    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private float moveInput;
    private bool isRunning;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- DIALOGUE LOCK CHECK ---
        if (isLockedByDialogue)
        {
            moveInput = 0;
            isRunning = false;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("Speed", 0);

            // Stop footsteps if dialogue starts mid-walk
            if (AudioManager.instance != null) AudioManager.instance.StopFootsteps();
            return;
        }

        // --- GROUND CHECK ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Mathf.Abs(rb.linearVelocity.y) < landingThreshold)
        {
            jumpCount = 0;
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }

        // --- INPUT (Disabled during attack) ---
        if (!isAttacking)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            isRunning = Input.GetKey(KeyCode.LeftShift);
        }
        else
        {
            moveInput = 0;
            isRunning = false;
        }

        // --- FOOTSTEP AUDIO LOGIC ---
        HandleMovementAudio();

        // --- ATTACK ---
        if (Input.GetKeyDown(attackKey) && isGrounded && !isAttacking)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("Attack");

            // Play Attack SFX
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(AudioManager.instance.attackSound);
        }

        // --- JUMP ---
        if (!isAttacking)
        {
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount++;

                // Play Jump SFX
                if (AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(AudioManager.instance.jumpSound);
            }

            if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }

        // --- FLIP CHARACTER ---
        if (!isAttacking)
        {
            if (moveInput > 0 && !facingRight) Flip();
            else if (moveInput < 0 && facingRight) Flip();
        }

        // --- ANIMATOR ---
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("VelocityY", rb.linearVelocity.y);
        anim.speed = isRunning ? 1.6f : 1f;
    }

    void HandleMovementAudio()
    {
        if (AudioManager.instance == null) return;

        // Condition: Player is grounded and actually moving horizontally
        if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f && !isAttacking)
        {
            AudioManager.instance.PlayFootsteps(isRunning);
        }
        else
        {
            AudioManager.instance.StopFootsteps();
        }
    }

    void FixedUpdate()
    {
        if (isAttacking || isLockedByDialogue) return;

        float targetSpeed = (isRunning ? runSpeed : walkSpeed) * moveInput;

        if (Mathf.Abs(moveInput) < 0.01f && transform.parent != null)
        {
            return;
        }

        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : acceleration * 2;
        float movement = speedDiff * accelRate * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
        DeActivateHitbox();
    }

    public void ActivateHitbox() => attackHitbox?.SetActive(true);
    public void DeActivateHitbox() => attackHitbox?.SetActive(false);

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}