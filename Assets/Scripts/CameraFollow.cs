using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1, -10);
    public float smoothTime = 0.3f;

    // Change to public so the Spawner can see it
    public Transform target;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // If target isn't assigned yet, don't do anything
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
    }

    //commented out for now, may be useful later, later
}