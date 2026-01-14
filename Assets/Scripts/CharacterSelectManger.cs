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
        selectedCharacter = character;

        // Update UI
        if (portraitImage != null) portraitImage.sprite = character.portrait;
        if (nameText != null) nameText.text = character.characterName;
        if (descriptionText != null) descriptionText.text = character.description;

        // Store selection for next scene
        PlayerPrefs.SetInt("SelectedCharacter", character.characterID);
        PlayerPrefs.Save();

        Debug.Log($"Selected Character: {character.characterName}");
    }

    public CharacterData GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    // --- MODIFIED: Now transitions to Level Select instead of Scene ---
    public void OnStartButtonClicked()
    {
        if (selectedCharacter == null)
        {
            Debug.LogWarning("No character selected!");
            return;
        }

        // Talk to the MenuManager to swap panels
        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowLevelSelect();
        }
        else
        {
            Debug.LogError("MenuManager not found in scene!");
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
        Debug.Log("Character selection cleared.");
    }

    public void OnBackToMainMenu()
    {
        ClearSelection();
        // If you are using MenuManager, you can call MenuManager.instance.ShowMainMenu() 
        // instead of reloading the scene to keep it fast.
        SceneManager.LoadScene("MainMenu");
    }

    public void DebugSelectedCharacter()
    {
        if (selectedCharacter != null)
            Debug.Log($"Currently Selected Character: {selectedCharacter.characterName}");
        else
            Debug.Log("No character currently selected.");
    }
}