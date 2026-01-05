using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [Header("Connection")]
    [Tooltip("Drag the specific Trap object here")]
    public SpikeTrap targetTrap;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if it's the player and hasn't been used yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (targetTrap != null)
            {
                targetTrap.ActivateTrap();
                hasTriggered = true; // Prevents the trap from firing repeatedly
            }
        }
    }
}