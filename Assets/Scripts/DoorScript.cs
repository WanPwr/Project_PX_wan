using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0); // How far the door moves up
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool shouldOpen = false;

    void Start()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition + openOffset;
    }

    void Update()
    {
        if (shouldOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    // This is the function your Universal Trigger will call
    public void OpenDoor()
    {
        shouldOpen = true;
    }
}