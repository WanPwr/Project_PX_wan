using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;

    [Header("UI References")]
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    private CharacterData selectedCharacter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SelectCharacter(CharacterData character)
    {
        // TRIGGER: Sound when clicking a character icon
        PlayClick();

        selectedCharacter = character;

        if (portraitImage != null) portraitImage.sprite = character.portrait;
        if (nameText != null) nameText.text = character.characterName;
        if (descriptionText != null) descriptionText.text = character.description;

        PlayerPrefs.SetInt("SelectedCharacter", character.characterID);
        PlayerPrefs.Save();
    }

    public void OnStartButtonClicked()
    {
        // TRIGGER: Sound when confirming character
        PlayClick();

        if (selectedCharacter == null)
        {
            Debug.LogWarning("No character selected!");
            return;
        }

        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowLevelSelect();
        }
    }

    public void OnBackToMainMenu()
    {
        // TRIGGER: Sound when going back
        PlayClick();

        ClearSelection();

        // Note: If MenuManager exists, it's better to use MenuManager.instance.ShowMainMenu()
        // so the scene doesn't reload and the audio doesn't glitch.
        SceneManager.LoadScene("MainMenu");
    }

    // --- HELPER FUNCTION ---
    private void PlayClick()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClickSound();
        }
    }

    public void ClearSelection()
    {
        selectedCharacter = null;
        if (portraitImage != null) portraitImage.sprite = null;
        if (nameText != null) nameText.text = "";
        if (descriptionText != null) descriptionText.text = "";

        PlayerPrefs.DeleteKey("SelectedCharacter");
        PlayerPrefs.Save();
    }
}