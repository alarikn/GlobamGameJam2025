using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<SoundEffect> effectList = new();

    public static AudioManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(string soundName)
    {
        foreach(var effect in effectList)
        {
            if(effect.soundName == soundName)
            {
                audioSource.volume = effect.volume;
                audioSource.PlayOneShot(effect.soundClip);
            }
        }
    }
}
