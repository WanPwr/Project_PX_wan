using UnityEngine;
using TMPro;
using System.Collections;

public class LevelGoal : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI objectiveText;
    public RectTransform objectivePanel;

    [Header("Slide Settings")]
    public Vector2 hiddenPosition = new Vector2(600, 0);
    public Vector2 visiblePosition = new Vector2(-20, 0);
    public float slideDuration = 0.5f;

    [Header("Current Goal Data")]
    public string targetItem = "None";
    public int neededAmount = 0;
    public string objectiveDescription = "";

    private PlayerInventory player;

    void Start()
    {
        if (objectivePanel != null)
            objectivePanel.anchoredPosition = hiddenPosition;

        FindPlayer();
        UpdateObjectiveUI();
    }

    void FindPlayer()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.GetComponent<PlayerInventory>();
    }

    public void SetNewObjective(string item, int amount, string description)
    {
        targetItem = item;
        neededAmount = amount;
        objectiveDescription = description;

        UpdateObjectiveUI();
        StopAllCoroutines();
        StartCoroutine(SlidePanel(visiblePosition));
    }

    public void ClearObjective()
    {
        targetItem = "None";
        neededAmount = 0;
        objectiveDescription = "Quest Complete!";
        UpdateObjectiveUI();

        StopAllCoroutines();
        StartCoroutine(WaitThenSlide(2.0f, hiddenPosition));
    }

    IEnumerator WaitThenSlide(float delay, Vector2 target)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SlidePanel(target));
    }

    IEnumerator SlidePanel(Vector2 targetPos)
    {
        float time = 0;
        Vector2 startPos = objectivePanel.anchoredPosition;
        while (time < slideDuration)
        {
            objectivePanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, time / slideDuration);
            time += Time.deltaTime;
            yield return null;
        }
        objectivePanel.anchoredPosition = targetPos;
    }

    void Update()
    {
        if (player == null) FindPlayer();

        if (player != null && targetItem != "None")
        {
            UpdateObjectiveUI();
            if (player.GetItemCount(targetItem) >= neededAmount)
            {
                objectiveDescription = "Goal Reached! Return to NPC.";
            }
        }
    }

    void UpdateObjectiveUI()
    {
        if (objectiveText != null)
        {
            string progress = "";
            if (targetItem != "None" && player != null)
            {
                progress = $"\nProgress: {player.GetItemCount(targetItem)}/{neededAmount}";
            }
            objectiveText.text = $"Goal: {objectiveDescription}{progress}";
        }
    }

    // Add this to LevelGoal.cs so PlayerSpawner can call it
    public void AssignPlayer(GameObject playerObj)
    {
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerInventory>();
            Debug.Log("LevelGoal: Player manually assigned by Spawner.");
        }
    }

    // Add this to your LevelGoal.cs script
    public void RefreshProgress()
    {
        UpdateObjectiveUI();
        Debug.Log("LevelGoal: UI Refreshed via Pickup.");
    }
}