using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject endScreenPanel;

    [Header("Scene Configuration")]
    [Tooltip("Exact name of the next stage (e.g., Stage2)")]
    public string nextLevelName;
    public string homeMenuName = "MainMenu";

    void Start()
    {
        // Always ensure the game is unpaused and panel is hidden at start
        Time.timeScale = 1f;
        if (endScreenPanel != null) endScreenPanel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);
            Time.timeScale = 0f; // Freeze game world logic
            
            // SFX for winning
            if (AudioManager.instance != null)
                AudioManager.instance.PlayClickSound(); 
        }
    }

    public void OnClickNextStage()
    {
        Time.timeScale = 1f; 
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogError("Next Level Name is missing in Inspector!");
        }
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(homeMenuName);
    }
}