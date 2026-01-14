using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderSync : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        // 1. Set the slider to match the CURRENT volume of the manager
        if (AudioManager.instance != null)
        {
            slider.value = AudioManager.instance.musicSource.volume;
        }

        // 2. Tell the slider to update the manager whenever it moves
        slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(value);
        }
    }
}