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

    //�����Ŭ�� �ε�
    void Start()
    {
        water1 = Resources.Load<AudioClip>("Audios/Water1");
        waterSound2 = Resources.Load<AudioClip>("Audios/WaterSound2");
    }

    //ȿ����, �� ���� ���
    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    //�ε巴�� ����� Ŭ�� ���(���࿡ �ڷ�ƾ ���� �߿� ������� ���´ٸ� ���Ƿ� �ڵ� �󿡼� ���������)
    //1. �÷��̽�ų ������ҽ�
    //2. �÷����� �����Ŭ��
    //3. �ε巴�� ����� �ð�
    //4. �ִ� ����
    //5. �ش� Ŭ���� ��������

    public Coroutine PlayAudioSmooth(AudioSource audioSource, AudioClip audioClip, float sTime, float maxVolume, bool isLoop)
    {
        return StartCoroutine(PlayAudioSmoothCor(audioSource, audioClip, sTime, maxVolume, isLoop));
    }

    //����� �ε巴�� �ߴ�
    //1. ��� ���� ����� �ҽ�
    //2. �ε巴�� ���� �ð�
    public void StopAudioSmooth(AudioSource audioSource, float sTime)
    {
        StartCoroutine(StopAudioSmoothCor(audioSource, sTime, 0));
    }

    //�ε巴�� ����� Ŭ�� ��� �ڷ�ƾ
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
    
    //�ε巴�� ����� Ŭ�� �ߴ� �ڷ�ƾ
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
