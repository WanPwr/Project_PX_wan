using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject endScreenPanel;

    [Header("Scene Configuration")]
    [Tooltip("Type the exact name of your next scene here")]
    public string nextLevelName;

    [Tooltip("Type the exact name of your main menu scene")]
    public string homeMenuName = "MainMenu";

    void Start()
    {
        // Ensure panel is hidden when the level starts
        if (endScreenPanel != null) endScreenPanel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        endScreenPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    // --- Button Functions ---

    public void OnClickNextStage()
    {
        Time.timeScale = 1f; // Unpause before loading!

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogError("Next Level Nam  e is empty in the Inspector!");
        }
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        // This is a "flexible" way to reload whatever scene you are currently in
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(homeMenuName);
    }
}