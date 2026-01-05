using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 targetPosition;

    void Start()
    {
        // Start by moving towards Point B
        if (pointA != null && pointB != null)
        {
            targetPosition = pointB.position;
        }
    }

    void Update()
    {
        // Move towards the target
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Use a 'Distance' check with a small buffer (0.1f)
        // If the platform is within 0.1 units of the point, it counts as "arrived"
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Explicitly swap targets
            if (targetPosition == pointB.position)
            {
                targetPosition = pointA.position;
            }
            else
            {
                targetPosition = pointB.position;
            }

            Debug.Log("Arrived at target! Swapping to: " + (targetPosition == pointA.position ? "Point A" : "Point B"));
        }
    }

    // This draws a green line in the editor so you can see the path
    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}