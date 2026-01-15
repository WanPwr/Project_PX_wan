using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource;

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
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        musicSource.volume = savedMusicVol;
        sfxSource.volume = savedSfxVol;
        footstepSource.volume = savedSfxVol;

        if (mainTheme != null)
        {
            PlayMusic(mainTheme);
        }
    }

    // --- MUSIC CONTROLS ---
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource == null || musicClip == null) return;

        // Prevent restarting if the song is already playing
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    // --- SFX CONTROLS ---
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    // --- HELPER METHODS FOR PLAYER (Fixes the errors) ---
    public void PlayJump() => PlaySFX(jumpSound);
    public void PlayAttack() => PlaySFX(attackSound);
    public void PlayClickSound() => PlaySFX(buttonClick);

    // --- MOVEMENT ---
    public void PlayFootsteps(bool isRunning)
    {
        if (footstepSource == null || walkSound == null || runSound == null) return;
        AudioClip selectedClip = isRunning ? runSound : walkSound;

        if (footstepSource.clip != selectedClip)
        {
            footstepSource.clip = selectedClip;
            footstepSource.Play();
        }
        else if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
        footstepSource.pitch = isRunning ? 1.2f : 1.0f;
    }

    public void StopFootsteps()
    {
        if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
    }

    // --- VOLUME ---
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        footstepSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}