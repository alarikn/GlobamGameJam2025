using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip baseMusic;
    [SerializeField] private AudioClip shopMusic;
    [SerializeField] private AudioSource musicPlayer;

    private float volume = 0.15f;
    public static MusicManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

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
        yield return null;

        //while(musicPlayer.volume > 0.02)
        //{
        //    musicPlayer.volume -= 0.02f;
        //    yield return new WaitForSeconds(0.03f);
        //    if (musicPlayer.volume <= 0) break;
        //}

        float timeStamp = musicPlayer.time;
        musicPlayer.Pause();
        musicPlayer.clip = songTo;
        musicPlayer.time = timeStamp;
        musicPlayer.Play();

        //while(musicPlayer.volume < volume)
        //{
        //    musicPlayer.volume += 0.02f;
        //    yield return new WaitForSeconds(0.03f);
        //    if (musicPlayer.volume >= volume) break;
        //}

        musicPlayer.volume = volume;
    }
}
