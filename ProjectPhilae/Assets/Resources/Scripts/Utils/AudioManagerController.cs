using UnityEngine;
using System;
using System.Linq;

public class AudioManagerController : Singleton<AudioManagerController>
{
    public static AudioManagerController Instance => (AudioManagerController)instance;
    public Sound[] sounds;

    void Awake()
    {
        base.Awake();
        foreach (var sound in sounds)
        {
            sound.SetSourceValues(gameObject.AddComponent<AudioSource>(),
                                    sound.clip,
                                    sound.volume,
                                    sound.pitch,
                                    sound.loop);
        }
    }

    private void Start()
    {
        PlaySound("MainTheme");
    }

    public void PlaySound(string soundName, float delay = 0f)
    {
        ValidateSoundName(soundName);

        var sound = Array.Find(sounds, s => s.name == soundName);

        if (delay == 0f)
            sound.Source.Play();
        else if (delay > 0f)
            sound.Source.PlayDelayed(delay);
    }

    private void ValidateSoundName(string soundName)
    {
        if (string.IsNullOrWhiteSpace(soundName))
            throw new ArgumentOutOfRangeException();

        if (!sounds.Any(sound => sound.name == soundName))
            Debug.LogWarning($"Sound: {soundName} not found!");
    }

    public void StopSound(string soundName)
    {
        ValidateSoundName(soundName);
        var sound = Array.Find(sounds, s => s.name == soundName);
        sound.Source.Stop();
    }

    public bool IsSoundPlaying(string soundName)
    {
        ValidateSoundName(soundName);
        var sound = Array.Find(sounds, s => s.name == soundName);
        return sound.Source.isPlaying;
    }
}
