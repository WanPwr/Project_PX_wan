using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip mainTheme;

    [Header("UI SFX Clips")]
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip errorPopup;

    [Header("Game SFX Clips")]
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip respawnSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // <--- ADD THIS. Stops the script immediately.
        }
    }

    void Start()
    {
        // Check if there is a theme assigned AND if it's NOT already playing
        // This prevents the "Restart" glitch when going back to the menu
        if (mainTheme != null)
        {
            if (musicSource.clip == mainTheme && musicSource.isPlaying)
            {
                // Already playing the right song, do nothing!
                return;
            }
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            musicSource.volume = savedVolume;
            // If you have a reference to the slider, you should update it too:
            // mySlider.value = savedVolume;

            PlayMusic(mainTheme);
        }
    }

    // --- MUSIC LOGIC ---
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource == null) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // --- SFX LOGIC ---
    public void PlaySFX(AudioClip sfxClip)
    {
        // CRITICAL FIX: Check if sfxSource still exists before playing
        if (sfxClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    // Add this to your AudioManager.cs
    public void SetMusicVolume(float volume)
    {
        // Clamp the value to be safe
        float finalVolume = Mathf.Clamp(volume, 0f, 1f);

        musicSource.volume = finalVolume;

        // Optional: If the volume is basically zero, force a mute 
        // to prevent "ghost" sounds
        musicSource.mute = (finalVolume <= 0.001f);

        PlayerPrefs.SetFloat("MusicVolume", finalVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }



    // Helper methods for Inspector/Buttons
    public void PlayHoverSound() => PlaySFX(buttonHover);
    public void PlayClickSound() => PlaySFX(buttonClick);
}