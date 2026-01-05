using UnityEngine;

public class UniversalTrigger : MonoBehaviour
{
    [Header("What should this trigger?")]
    public MovingPlatform movingPlatform;
    public SpikeTrap spikeTrap;
    public DoorScript door;

    [Header("Trigger Settings")]
    [Tooltip("If true, you MUST press K. If false, it triggers as soon as you touch it.")]
    public bool isInteractable = false;

    [Tooltip("If true, this can only be activated once.")]
    public bool triggerOnlyOnce = false;

    private bool hasTriggered = false;
    private bool playerInZone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            // ONLY trigger on touch if isInteractable is FALSE
            if (!isInteractable)
            {
                ExecuteTriggerActions();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    void Update()
    {
        // Check for 'K' press ONLY if we are interactable and player is nearby
        if (isInteractable && playerInZone)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                ExecuteTriggerActions();
            }
        }
    }

    private void ExecuteTriggerActions()
    {
        // Stop if it's a one-time thing and already done
        if (triggerOnlyOnce && hasTriggered) return;

        // 1. Toggle Platform
        if (movingPlatform != null)
            movingPlatform.enabled = !movingPlatform.enabled;

        // 2. Activate Trap
        if (spikeTrap != null)
            spikeTrap.ActivateTrap();

        // 3. Open Door
        if (door != null)
            door.OpenDoor();

        hasTriggered = true;
        Debug.Log("Action executed via " + (isInteractable ? "Interaction (K)" : "Touch"));
    }
}