using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip water1;
    public AudioClip water2;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }


    public void PlayAudioSmooth(AudioSource audioSource, AudioClip audioClip, float sTime, float maxVolume)
    {
        StartCoroutine(PlayAudioSmoothCor(audioSource, audioClip, sTime, maxVolume));
    }

    public void StopAudioSmooth(AudioSource audioSource, float sTime, float volume)
    {
        StopCoroutine(StopAudioSmoothCor(audioSource, sTime, volume));
    }
    private IEnumerator PlayAudioSmoothCor(AudioSource audioSource, AudioClip audioClip, float sTime, float maxVolume)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        float vol = 0;
        while(audioSource.volume < maxVolume)
        {
            vol += Time.deltaTime / sTime;
            audioSource.volume = vol;
            yield return null;
        }
    }

    private IEnumerator StopAudioSmoothCor(AudioSource audioSource, float sTime, float volume)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / sTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = volume;
    }
}
