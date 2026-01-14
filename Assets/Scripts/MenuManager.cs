using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject characterSelectPanel;
    public GameObject settingsPanel;
    public GameObject levelSelectPanel; // <-- NEW PANEL

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ShowMainMenu();
    }

    void HideAll()
    {
        mainPanel.SetActive(false);
        characterSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAll();
        mainPanel.SetActive(true);
        PlayClick();
    }

    public void ShowCharacterSelect()
    {
        HideAll();
        characterSelectPanel.SetActive(true);
        PlayClick();
    }

    public void ShowLevelSelect()
    {
        HideAll();
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
        PlayClick();
    }

    public void ShowSettings()
    {
        HideAll();
        settingsPanel.SetActive(true);
        PlayClick();
    }

    // New function to load specific levels from buttons
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PlayClick()
    {
        if (AudioManager.instance != null && AudioManager.instance.sfxSource != null)
        {
            AudioManager.instance.PlayClickSound();
        }
    }
}