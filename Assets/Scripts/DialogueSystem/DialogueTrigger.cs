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
                CheckQuestStatusAndTalk();
            }
        }

        void CheckQuestStatusAndTalk()
        {
            LevelGoal goal = Object.FindFirstObjectByType<LevelGoal>();
            PlayerInventory inv = Object.FindFirstObjectByType<PlayerInventory>();

            // 1. Give Quest for the first time
            if (!hasStartedQuest)
            {
                manager.StartDialogue(questGiverDialogue);
                hasStartedQuest = true;
            }
            // 2. Already started, checking if finished
            else if (hasStartedQuest && !hasFinishedQuest)
            {
                if (goal != null && inv != null && inv.GetItemCount(goal.targetItem) >= goal.neededAmount)
                {
                    manager.StartDialogue(completedDialogue);
                    hasFinishedQuest = true;

                    // UNLOCK THE DOOR
                    if (doorToOpen != null) doorToOpen.OpenDoor();

                    // ACTIVATE TRIGGER
                    if (triggerToActivate != null) triggerToActivate.Interact();

                    // Clear UI
                    goal.ClearObjective();
                }
                else
                {
                    manager.StartDialogue(inProgressDialogue);
                }
            }
            // 3. Quest already finished (Post-quest chat)
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