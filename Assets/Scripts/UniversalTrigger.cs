using UnityEngine;

public class UniversalTrigger : MonoBehaviour
{
    [Header("What should this trigger?")]
    public MovingPlatform movingPlatform;
    public SpikeTrap spikeTrap;
    public DoorScript door;

    [Header("Trigger Settings")]
    public bool isInteractable = false;
    public bool triggerOnlyOnce = false;

    private bool hasTriggered = false;
    private bool playerInZone = false;

    // --- ADD THIS FUNCTION TO FIX THE ERROR ---
    public void Interact()
    {
        ExecuteTriggerActions();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (!isInteractable) ExecuteTriggerActions();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInZone = false;
    }

    void Update()
    {
        if (isInteractable && playerInZone && Input.GetKeyDown(KeyCode.K))
        {
            ExecuteTriggerActions();
        }
    }

    private void ExecuteTriggerActions()
    {
        if (triggerOnlyOnce && hasTriggered) return;

        if (movingPlatform != null) movingPlatform.enabled = !movingPlatform.enabled;
        if (spikeTrap != null) spikeTrap.ActivateTrap();
        if (door != null) door.OpenDoor();

        hasTriggered = true;
    }
}