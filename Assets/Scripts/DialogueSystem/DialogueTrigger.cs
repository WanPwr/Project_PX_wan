using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Conversation Phases")]
    public DialogueData questGiverDialogue;
    public DialogueData inProgressDialogue;
    public DialogueData completedDialogue;

    [Header("Quest Rewards")]
    [Tooltip("Drag the Door or the UniversalTrigger here to unlock it!")]
    public DoorScript doorToOpen;
    public UniversalTrigger triggerToActivate;

    [Header("References")]
    public DialogueManager manager;
    public GameObject promptUI;
    public KeyCode interactionKey = KeyCode.K;

    private bool isPlayerInRange = false;
    private bool hasStartedQuest = false;
    private bool hasFinishedQuest = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            // --- THE FIX ---
            // 1. Find the player
            PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();

            if (player != null && player.isLockedByDialogue)
            {
                // 2. If already talking, don't restart dialogue! 
                // Instead, tell the player script to show the error message.
                // Note: We need a public method in PlayerMovement for this, 
                // or just let PlayerMovement's own Update handle the detection.
                return;
            }

            // 3. If NOT locked, proceed with normal talk
            CheckQuestStatusAndTalk();
        }
    }

    void CheckQuestStatusAndTalk()
    {
        LevelGoal goal = Object.FindFirstObjectByType<LevelGoal>();
        PlayerInventory inv = Object.FindFirstObjectByType<PlayerInventory>();

        if (!hasStartedQuest)
        {
            manager.StartDialogue(questGiverDialogue);
            hasStartedQuest = true;
        }
        else if (hasStartedQuest && !hasFinishedQuest)
        {
            if (goal != null && inv != null && inv.GetItemCount(goal.targetItem) >= goal.neededAmount)
            {
                manager.StartDialogue(completedDialogue);
                hasFinishedQuest = true;

                if (doorToOpen != null) doorToOpen.OpenDoor();
                if (triggerToActivate != null) triggerToActivate.Interact();

                goal.ClearObjective();
            }
            else
            {
                manager.StartDialogue(inProgressDialogue);
            }
        }
        else
        {
            manager.StartDialogue(completedDialogue);
        }

        if (promptUI != null) promptUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}