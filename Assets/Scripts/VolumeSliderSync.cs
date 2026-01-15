using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderSync : MonoBehaviour
{
    public enum AudioType { Music, SFX }
    public AudioType type;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        // 1. Get the saved value first (Safe, doesn't need AudioManager)
        string key = (type == AudioType.Music) ? "MusicVolume" : "SFXVolume";
        float savedVol = PlayerPrefs.GetFloat(key, 0.5f);

        slider.value = savedVol;

        // 2. Only apply to AudioManager if it actually exists right now
        ApplyVolume(savedVol);

        // 3. Listen for future changes
        slider.onValueChanged.AddListener(delegate { ApplyVolume(slider.value); });
    }

    void ApplyVolume(float val)
    {
        if (AudioManager.instance != null)
        {
            if (type == AudioType.Music)
                AudioManager.instance.SetMusicVolume(val);
            else
                AudioManager.instance.SetSFXVolume(val);
        }
    }
}