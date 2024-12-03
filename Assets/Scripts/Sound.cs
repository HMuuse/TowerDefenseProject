using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource audioSource;

    public bool isLooping;
}

