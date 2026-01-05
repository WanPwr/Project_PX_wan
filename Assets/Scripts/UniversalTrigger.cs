using UnityEngine;

public class UniversalTrigger : MonoBehaviour
{
    [Header("What should this trigger?")]
    // You can drag a Platform, a Trap, or a Door here
    public MovingPlatform movingPlatform;
    public SpikeTrap spikeTrap;

    [Header("Settings")]
    public bool triggerOnlyOnce = false;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerOnlyOnce && hasTriggered) return;

            // Trigger the Platform if one is assigned
            if (movingPlatform != null)
            {
                movingPlatform.enabled = !movingPlatform.enabled;
            }

            // Trigger the Trap if one is assigned
            if (spikeTrap != null)
            {
                spikeTrap.ActivateTrap();
            }

            hasTriggered = true;
            Debug.Log("Universal Trigger Activated!");
        }
    }
}