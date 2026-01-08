using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
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
    public float landingThreshold = 0.1f;

    [Header("UI Popups")]
    public GameObject movementWarningUI; // Drag your "Error Message" UI Panel/Text here
    public float warningDuration = 1.5f;

    [Header("Attack Settings")]
    public KeyCode attackKey = KeyCode.J;
    public GameObject attackHitbox;

    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;
    private bool isRunning;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Hide warning at start
        if (movementWarningUI != null) movementWarningUI.SetActive(false);
    }

    void Update()
    {
        // --- DIALOGUE LOCK CHECK & WARNING LOGIC ---
        if (isLockedByDialogue)
        {
            // Check for A, D (Horizontal Axis) or specific keys J, K, L, or Jump
            bool triedToMove = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f;
            bool triedToAction = Input.GetKeyDown(KeyCode.J) ||
                                 Input.GetKeyDown(KeyCode.K) ||
                                 Input.GetKeyDown(KeyCode.L) ||
                                 Input.GetButtonDown("Jump");

            if (triedToMove || triedToAction)
            {
                ShowMovementWarning();
            }

            // Stop all movement while locked
            moveInput = 0;
            isRunning = false;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("Speed", 0);
            return; // Important: Stops the rest of the script from running
        }

        // --- NORMAL GROUND CHECK ---
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

        // --- NORMAL INPUT ---
        if (!isAttacking)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            isRunning = Input.GetKey(KeyCode.LeftShift);
        }

        // --- ATTACK ---
        if (Input.GetKeyDown(attackKey) && isGrounded && !isAttacking)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("Attack");
        }

        // --- JUMP ---
        if (!isAttacking)
        {
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount++;
            }

            if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }

        // --- FLIP ---
        if (!isAttacking)
        {
            if (moveInput > 0 && !facingRight) Flip();
            else if (moveInput < 0 && facingRight) Flip();
        }

        // --- ANIMATOR ---
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("VelocityY", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (isAttacking || isLockedByDialogue) return;

        float targetSpeed = (isRunning ? runSpeed : walkSpeed) * moveInput;

        // Parenting/Platform Logic
        if (Mathf.Abs(moveInput) < 0.01f && transform.parent != null) return;

        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : acceleration * 2;
        float movement = speedDiff * accelRate * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void ShowMovementWarning()
    {
        if (movementWarningUI != null)
        {
            movementWarningUI.SetActive(true);
            CancelInvoke("HideWarning");
            Invoke("HideWarning", warningDuration);
            Debug.Log("Movement is locked during dialogue!");
        }
        else
        {
            Debug.LogWarning("Movement warning UI is not assigned!");
        }
    }

    private void HideWarning()
    {
        if (movementWarningUI != null) movementWarningUI.SetActive(false);
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
        if (attackHitbox != null) attackHitbox.SetActive(false);
    }
}