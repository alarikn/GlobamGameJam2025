using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip baseMusic;
    [SerializeField] private AudioClip shopMusic;
    [SerializeField] private AudioSource musicPlayer;

    private float volume;
    public static MusicManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShopMusic(bool value)
    {
        if (value)
        {
            StartCoroutine(FadeMusic(shopMusic));
        }
        else
        {
            StartCoroutine(FadeMusic(baseMusic));
        }
    }

    private IEnumerator FadeMusic(AudioClip songTo)
    {
        volume = musicPlayer.volume;
        while(musicPlayer.volume > 0)
        {
            musicPlayer.volume -= 0.02f;
            yield return new WaitForSeconds(0.03f);
        }

        float timeStamp = musicPlayer.time;
        musicPlayer.Pause();
        musicPlayer.clip = songTo;
        musicPlayer.Play();
        musicPlayer.time = timeStamp;

        while(musicPlayer.volume < volume)
        {
            musicPlayer.volume += 0.02f;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
