using UnityEngine;

public class LevelMusicTrigger : MonoBehaviour
{
    public AudioClip levelBGM;

    void Start()
    {
        // When the level starts, it finds the PERSISTENT AudioManager 
        // and tells it to change the song.
        if (AudioManager.instance != null && levelBGM != null)
        {
            AudioManager.instance.PlayMusic(levelBGM);
        }
    }
}