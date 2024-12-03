using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple AudioManager instances found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        // Initialize audio sources for each sound
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.isLooping;
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            return;
        }
        sound.audioSource.Play();
    }

    public void Pause(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound with name {name} not found!");
            return;
        }
        sound.audioSource.Pause();
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            return;
        }
        sound.audioSource.Stop();
    }

    // Global Volume setting function
    public void SetVolume(float volume)
    {
        foreach (Sound sound in sounds)
        {
            sound.audioSource.volume = volume;
        }
    }
}
