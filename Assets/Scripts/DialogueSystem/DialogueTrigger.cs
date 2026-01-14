using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Conversation Phases")]
    public DialogueData questGiverDialogue;
    public DialogueData inProgressDialogue;
    public DialogueData completedDialogue;

    [Header("Quest Rewards")]
    public DoorScript doorToOpen;
    public UniversalTrigger triggerToActivate;

    [Header("End Level Settings")]
    [Tooltip("Drag the LevelController object here to trigger the Win Screen")]
    public LevelController levelController;
    public bool isFinalQuestOfLevel = true;
    public float delayBeforeWinScreen = 2.0f;

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
            CheckQuestStatusAndTalk();
        }
    }

    void CheckQuestStatusAndTalk()
    {
        LevelGoal goal = Object.FindFirstObjectByType<LevelGoal>();
        PlayerInventory inv = Object.FindFirstObjectByType<PlayerInventory>();

        // 1. Give Quest
        if (!hasStartedQuest)
        {
            manager.StartDialogue(questGiverDialogue);
            hasStartedQuest = true;
        }
        // 2. Check Progress
        else if (hasStartedQuest && !hasFinishedQuest)
        {
            if (goal != null && inv != null && inv.GetItemCount(goal.targetItem) >= goal.neededAmount)
            {
                // SUCCESS
                manager.StartDialogue(completedDialogue);
                hasFinishedQuest = true;

                if (doorToOpen != null) doorToOpen.OpenDoor();
                if (triggerToActivate != null) triggerToActivate.Interact();

                goal.ClearObjective();

                // Trigger the end of the level
                if (isFinalQuestOfLevel && levelController != null)
                {
                    Invoke("TriggerWin", delayBeforeWinScreen);
                }
            }
            else
            {
                manager.StartDialogue(inProgressDialogue);
            }
        }
        // 3. Post-Quest
        else
        {
            manager.StartDialogue(completedDialogue);
        }

        if (promptUI != null) promptUI.SetActive(false);
    }

    private void TriggerWin()
    {
        levelController.ShowEndScreen();
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