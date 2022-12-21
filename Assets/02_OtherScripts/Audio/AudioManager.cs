using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Music Settings")]
    [SerializeField] private float musicFadeDuration = 1f;

    [Header("Audiomanager Settings")]
    [SerializeField] private AudioObject[] audioObjects;

    private List<AudioObject> music = new();

    protected override void SingletonAwake()
    {
        foreach (AudioObject s in audioObjects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.PlayOnAwake;

            if (s.isMusic)
            {
                music.Add(s);
            }
        }
    }

    public bool IsPlaying(string name)
    {
        AudioObject s = Array.Find(audioObjects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not Found");
            return false;
        }
        else if (!s.source.isPlaying)
        {
            return false;
        }

        return true;
    }

    public void Play(string name)
    {
        AudioObject s = Array.Find(audioObjects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not Found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        AudioObject s = Array.Find(audioObjects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not Found");
            return;
        } else if (!s.source.isPlaying)
        {
            return;
        }
        s.source.Stop();
    }

    public void PlayFadeIn(string name, float fadeDuration)
    {
        AudioObject s = Array.Find(audioObjects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not Found");
            return;
        }
        s.source.Play();
        
        s.source.volume = 0;
        StartCoroutine(FadingSound(s.source, s.volume, fadeDuration));
    }

    public void StopFadeOut(string name, float fadeDuration)
    {
        AudioObject s = Array.Find(audioObjects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not Found");
            return;
        }
        else if (!s.source.isPlaying)
        {
            return;
        }

        StartCoroutine(FadingSound(s.source, 0, fadeDuration));
    }

    public void PlayMusic(string name)
    {
        AudioObject s = Array.Find(audioObjects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not Found");
            return;
        } else if (!s.isMusic)
        {
            Debug.Log($"Sound: {name} isn't marked as music");
            return;
        }

        foreach (AudioObject m in music)
        {
            if (m.source.isPlaying)
            {
                if (m.name == name) return;

                // fade old music out and new in
                StartCoroutine(FadingSound(m.source, 0, musicFadeDuration, () => PlayFadeIn(name, musicFadeDuration)));
                return;
            }
        }

        PlayFadeIn(name, musicFadeDuration);
    }

    private IEnumerator FadingSound(AudioSource audioSource, float targetVolume, float fadeDuration, Action onComplete = null)
    {
        float elapsed_time = 0;
        float startVolume = audioSource.volume;

        while (elapsed_time <= fadeDuration)
        {
            float newVolume = Mathf.Lerp(startVolume, targetVolume, Mathf.Pow((elapsed_time / fadeDuration), 2));

            audioSource.volume = newVolume;

            yield return null;

            elapsed_time += Time.deltaTime;
        }

        if (audioSource.volume < 0.01f) audioSource.Stop();
        onComplete?.Invoke();
    }
}
