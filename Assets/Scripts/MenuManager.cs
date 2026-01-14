using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject mainPanel;
    public GameObject characterSelectPanel;
    public GameObject settingsPanel;

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
    }

    public void ShowMainMenu()
    {
        HideAll();
        mainPanel.SetActive(true);

        // Check if the instance exists AND if the AudioSource hasn't been destroyed
        if (AudioManager.instance != null && AudioManager.instance.sfxSource != null)
        {
            AudioManager.instance.PlayClickSound();
        }
    }
    public void ShowCharacterSelect()
    {
        HideAll();
        characterSelectPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAll();
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
