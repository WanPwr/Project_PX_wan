using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public GameObject spacePrompt;

    [Header("Settings")]
    public float typingSpeed = 0.04f;
    [Tooltip("Play the typing sound every X characters to avoid noise overload")]
    public int typingSoundFrequency = 2;

    private Queue<DialogueLine> lines;
    private bool isTyping = false;
    private string currentFullText;
    private DialogueData currentDialogueData;
    private bool canAdvance = false;

    void Awake()
    {
        lines = new Queue<DialogueLine>();
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (spacePrompt != null) spacePrompt.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogueData = dialogue;
        TogglePlayerMovement(false);
        dialogueBox.SetActive(true);
        canAdvance = false;

        lines.Clear();
        foreach (DialogueLine line in dialogue.conversation)
        {
            lines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentFullText;
            isTyping = false;
            if (spacePrompt != null) spacePrompt.SetActive(true);
            return;
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        // --- NEW: Play "Next" SFX when advancing ---
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(AudioManager.instance.dialogueNextSound);

        if (spacePrompt != null) spacePrompt.SetActive(false);
        DialogueLine currentLine = lines.Dequeue();

        nameText.text = currentLine.characterName;
        portraitImage.sprite = currentLine.characterPortrait;
        currentFullText = currentLine.text;

        StartCoroutine(TypeSentence(currentFullText));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        int charCount = 0;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            charCount++;

            // --- OPTIONAL: Subtle Typing Blip ---
            // We play the sound every few characters so it sounds like "chatter" 
            // instead of one continuous beep.
            if (charCount % typingSoundFrequency == 0 && AudioManager.instance != null)
            {
                // You can use dialogueNextSound or a dedicated 'typing' sound here
                AudioManager.instance.PlaySFX(AudioManager.instance.dialogueNextSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        if (spacePrompt != null) spacePrompt.SetActive(true);
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        if (spacePrompt != null) spacePrompt.SetActive(false);

        if (currentDialogueData != null && currentDialogueData.givesQuest)
        {
            LevelGoal goal = Object.FindFirstObjectByType<LevelGoal>();
            if (goal != null)
            {
                goal.SetNewObjective(
                    currentDialogueData.questItemName,
                    currentDialogueData.questAmount,
                    currentDialogueData.questDescription
                );
            }
        }

        TogglePlayerMovement(true);
    }

    private void TogglePlayerMovement(bool canMove)
    {
        PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.isLockedByDialogue = !canMove;
        }
    }

    void Update()
    {
        if (!dialogueBox.activeSelf) return;

        if (!canAdvance)
        {
            if (!Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.Space))
            {
                canAdvance = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }
}