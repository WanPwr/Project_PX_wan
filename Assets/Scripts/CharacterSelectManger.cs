using UnityEngine;
using UnityEngine.UI;       // ← required for Image
using TMPro;               // ← required for TMP_Text
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;

    [Header("UI References")]
    public Image portraitImage;
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text descriptionText;

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

    // Call this from the Start button OnClick
    public void OnStartButtonClicked()
    {
        if (selectedCharacter == null)
        {
            Debug.LogWarning("No character selected!");
            return;
        }

        // Example: Load your first game level
        SceneManager.LoadScene("TRY2");
    }

    //funtion to clear selection
    public void ClearSelection()
    {
        selectedCharacter = null;

        // Clear UI
        if (portraitImage != null) portraitImage.sprite = null;
        if (nameText != null) nameText.text = "";
        if (descriptionText != null) descriptionText.text = "";

        // Clear PlayerPrefs
        PlayerPrefs.DeleteKey("SelectedCharacter");
        PlayerPrefs.Save();

        Debug.Log("Character selection cleared.");
    }

    //just like clear selection but happens when player presses esc to go back to main menu
    public void OnBackToMainMenu()
    {
        ClearSelection();
        SceneManager.LoadScene("MainMenu");
    }

    //a function to debug the current selected character
    public void DebugSelectedCharacter()
    {
        if (selectedCharacter != null)
        {
            Debug.Log($"Currently Selected Character: {selectedCharacter.characterName}");
        }
        else
        {
            Debug.Log("No character currently selected.");
        }
    }
}
