using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AudioData
{
    [Header("들어갈 사운드")]
    public AudioClip audio;
    [Header("사운드 이름(배경음은 해당 씬 이름이랑 동일하게)")]
    public string audioName;
}
public class SoundManager : MonoBehaviour
{
    public AudioData[] audioDatas;

    public Dictionary<string, AudioClip> audioDictionary;

    [SerializeField] private AudioSource[] audioSources;
    //Manager.Instance.soundManager.Play(Manager.Instance.soundManager.audioDictionary["이름"], false);
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

        if (_isBgm) // BGM 배경음악 재생
        {

            if (audioSources[0].isPlaying)
                audioSources[0].Stop();

            audioSources[0].pitch = pitch;
            audioSources[0].clip = audioClip;
            audioSources[0].Play();
        }
        else // Effect 효과음 재생
        {
            audioSources[1].pitch = pitch;
            audioSources[1].PlayOneShot(audioClip);
        }
    }
}
