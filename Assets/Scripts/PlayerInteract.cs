using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // This stores the specific trigger the player is currently touching
    private MovingPlatformTrigger currentTrigger;

    void Update()
    {
        // Only allow pressing K if we are actually inside a trigger zone
        if (Input.GetKeyDown(KeyCode.K) && currentTrigger != null)
        {
            currentTrigger.Interact();
        }
    }

    // These tell the player WHICH specific trigger they are standing in
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovingPlatformTrigger trigger))
        {
            currentTrigger = trigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MovingPlatformTrigger trigger))
        {
            // Only clear it if we are leaving the one we were just using
            if (currentTrigger == trigger)
            {
                currentTrigger = null;
            }
        }
    }
}