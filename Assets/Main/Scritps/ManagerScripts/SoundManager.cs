using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AudioData
{
    [Header("�� ����")]
    public AudioClip audio;
    [Header("���� �̸�(������� �ش� �� �̸��̶� �����ϰ�)")]
    public string audioName;
}
public class SoundManager : MonoBehaviour
{
    public AudioData[] audioDatas;

    public Dictionary<string, AudioClip> audioDictionary;

    [SerializeField] private AudioSource[] audioSources;
    //Manager.Instance.soundManager.Play(Manager.Instance.soundManager.audioDictionary["�̸�"], false);
    private void Awake()
    {
        audioDictionary = new Dictionary<string, AudioClip>();
        AudioDataInit();
    }

    private void AudioDataInit()
    {
        for (int i = 0; i < audioDatas.Length; i++) audioDictionary.Add(audioDatas[i].audioName, audioDatas[i].audio);

        //Play(audioDictionary["MainBGM"], true);
    }

    public void Play(AudioClip audioClip, bool _isBgm, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (_isBgm) // BGM ������� ���
        {

            if (audioSources[0].isPlaying)
                audioSources[0].Stop();

            audioSources[0].pitch = pitch;
            audioSources[0].clip = audioClip;
            audioSources[0].Play();
        }
        else // Effect ȿ���� ���
        {
            audioSources[1].pitch = pitch;
            audioSources[1].PlayOneShot(audioClip);
        }
    }
}
