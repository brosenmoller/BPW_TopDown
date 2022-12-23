using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Music Settings")]
    [SerializeField] private float musicFadeDuration = 1f;

    private List<AudioObject> music = new();

    public void PlayMusic(AudioObject audioObject)
    {
        foreach (AudioObject m in music)
        {
            if (m.Source.isPlaying)
            {
                if (m.name == name) return;

                // fade old music out and new in
                StartCoroutine(FadingSound(m.Source, 0, musicFadeDuration, () => m.PlayFadeIn(musicFadeDuration)));
                return;
            }
        }
    }

    public IEnumerator FadingSound(AudioSource audioSource, float targetVolume, float fadeDuration, Action onComplete = null)
    {
        float elapsedTime = 0;
        float startVolume = audioSource.volume;

        while (elapsedTime <= fadeDuration)
        {
            float newVolume = Mathf.Lerp(startVolume, targetVolume, Mathf.Pow(elapsedTime / fadeDuration, 2));

            audioSource.volume = newVolume;

            yield return null;

            elapsedTime += Time.deltaTime;
        }

        if (audioSource.volume < 0.01f) audioSource.Stop();
        onComplete?.Invoke();
    }
}
