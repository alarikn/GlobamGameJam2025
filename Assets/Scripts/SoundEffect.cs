using UnityEngine;

[CreateAssetMenu(fileName = "New SoundEffect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public string soundName;
    public AudioClip soundClip;
    public float volume = 1.0f;
    public bool randomizePitch;
}
