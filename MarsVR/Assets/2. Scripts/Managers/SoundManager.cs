using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [HideInInspector] public AudioClip water1;
    [HideInInspector] public AudioClip waterSound2;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        DontDestroyOnLoad(instance);

    }

    //오디오클립 로드
    void Start()
    {
        water1 = Resources.Load<AudioClip>("Audios/Water1");
        waterSound2 = Resources.Load<AudioClip>("Audios/WaterSound2");
    }

    //효과음, 한 번만 재생
    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    //부드럽게 오디오 클립 재생(만약에 코루틴 실행 중에 오디오가 끊는다면 임의로 코드 상에서 끊어줘야함)
    //1. 플레이시킬 오디오소스
    //2. 플레이할 오디오클립
    //3. 부드럽게 재생할 시간
    //4. 최대 볼륨
    //5. 해당 클립을 루프할지

    public Coroutine PlayAudioSmooth(AudioSource audioSource, AudioClip audioClip, float sTime, float maxVolume, bool isLoop)
    {
        return StartCoroutine(PlayAudioSmoothCor(audioSource, audioClip, sTime, maxVolume, isLoop));
    }

    //오디오 부드럽게 중단
    //1. 재생 중인 오디오 소스
    //2. 부드럽게 끊을 시간
    public void StopAudioSmooth(AudioSource audioSource, float sTime)
    {
        StartCoroutine(StopAudioSmoothCor(audioSource, sTime, 0));
    }

    //부드럽게 오디오 클립 재생 코루틴
    private IEnumerator PlayAudioSmoothCor(AudioSource audioSource, AudioClip audioClip, float sTime, float maxVolume, bool isLoop)
    {
        audioSource.clip = audioClip;
        audioSource.loop = isLoop;
        audioSource.Play();
        float vol = 0;
        while(audioSource.volume < maxVolume)
        {
            vol += Time.deltaTime / sTime;
            audioSource.volume = vol;
            yield return null;
        }
    }
    
    //부드럽게 오디오 클립 중단 코루틴
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
