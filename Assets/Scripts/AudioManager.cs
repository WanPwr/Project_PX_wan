using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource; // Dedicated source for looping movement

    [Header("BGM Clips")]
    public AudioClip mainTheme;

    [Header("UI & Dialogue SFX")]
    public AudioClip buttonClick;
    public AudioClip npcInteractSound;
    public AudioClip dialogueNextSound;

    [Header("Game SFX Clips")]
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip respawnSound;

    [Header("Movement SFX Clips")]
    public AudioClip walkSound;
    public AudioClip runSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // IMPORTANT: Disable the object so it stops running logic 
            // while waiting to be destroyed at the end of the frame.
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load Saved Volumes from PlayerPrefs
        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        musicSource.volume = savedMusicVol;
        sfxSource.volume = savedSfxVol;
        footstepSource.volume = savedSfxVol;

        // Start Background Music
        if (mainTheme != null)
        {
            if (!(musicSource.clip == mainTheme && musicSource.isPlaying))
            {
                PlayMusic(mainTheme);
            }
        }
    }

    // --- MUSIC CONTROLS ---
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource == null || musicClip == null) return;
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // --- ONE-SHOT SFX CONTROLS (Jumps, Clicks, Attacks) ---
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    // --- MOVEMENT SFX CONTROLS (Looping) ---
    public void PlayFootsteps(bool isRunning)
    {
        if (footstepSource == null || walkSound == null || runSound == null) return;

        AudioClip selectedClip = isRunning ? runSound : walkSound;

        // Change clip only if it's different to prevent "stuttering" restarts
        if (footstepSource.clip != selectedClip)
        {
            footstepSource.clip = selectedClip;
            footstepSource.Play();
        }
        else if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }

        // Slight pitch increase when running makes it feel more energetic
        footstepSource.pitch = isRunning ? 1.2f : 1.0f;
    }

    public void StopFootsteps()
    {
        if (footstepSource != null && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

    // --- VOLUME SETTINGS (Call these from your Options Menu) ---
    public void SetMusicVolume(float volume)
    {
        float finalVolume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = finalVolume;
        PlayerPrefs.SetFloat("MusicVolume", finalVolume);
    }

    public void SetSFXVolume(float volume)
    {
        float finalVolume = Mathf.Clamp(volume, 0f, 1f);
        sfxSource.volume = finalVolume;
        footstepSource.volume = finalVolume;
        PlayerPrefs.SetFloat("SFXVolume", finalVolume);
    }

    // Helper methods for easy button linking
    public void PlayClickSound() => PlaySFX(buttonClick);
}