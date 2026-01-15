using UnityEngine;
using UnityEngine.UI;

public class SFXSliderSync : MonoBehaviour
{
    private Slider sfxSlider;

    void Start()
    {
        sfxSlider = GetComponent<Slider>();

        // 1. Load the saved volume so the slider handle is in the right spot
        float savedVol = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        sfxSlider.value = savedVol;

        // 2. Add a listener to call the AudioManager when the slider moves
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetSFXVolume(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSFXVolume(value);
        }
    }
}