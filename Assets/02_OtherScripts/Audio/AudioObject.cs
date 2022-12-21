using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioObject : ScriptableObject
{
    public AudioClip clip;

    public bool loop;
    public bool PlayOnAwake;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;

    public AudioMixerGroup audioMixer;
    public bool isMusic;
}
