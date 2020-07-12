
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    private AudioSource source;

    public AudioSource Source => source;

    public void SetSourceValues(AudioSource source,  AudioClip clip, float volume, float pitch, bool loop)
    {
        this.source = source;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }
}
