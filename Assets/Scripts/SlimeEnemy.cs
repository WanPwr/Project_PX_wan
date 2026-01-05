using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Jump")]
    public float jumpForce = 5f;
    public float jumpInterval = 1.5f; // seconds between jumps

    private Transform targetPoint;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float jumpTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        targetPoint = pointB;
        jumpTimer = jumpInterval;
    }

    void Update()
    {
        if (targetPoint == null) return;

        // Move horizontally
        Vector2 pos = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(pos);

        // Flip sprite
        sr.flipX = transform.position.x < targetPoint.position.x ? false : true;

        // Switch target point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }

        // Handle jumping
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimer = jumpInterval;
        }
    }

    bool IsGrounded()
    {
        // Raycast down to detect ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }

}
