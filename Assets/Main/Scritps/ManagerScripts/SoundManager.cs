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

    public string curBGM;

    private void Update()
    {
        EndBGM();
    }

    private void EndBGM()
    {
        if (!audioSources[0].isPlaying)
        {
            Play(curBGM, true); 
        }
    }

    public void Init()
    {
        audioDictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < audioDatas.Length; i++) audioDictionary.Add(audioDatas[i].audioName, audioDatas[i].audio);

        Play("Stage_1", true);
    }

    public void Play(string name, bool _isBgm, float pitch = 1.0f)
    {
        if (audioDictionary[name] == null)
            return;



        if (_isBgm) // BGM 배경음악 재생
        {
            curBGM = name;
            if (audioSources[0].isPlaying)
                audioSources[0].Stop();

            audioSources[0].pitch = pitch;
            audioSources[0].clip = audioDictionary[name];
            audioSources[0].Play();
        }
        else // Effect 효과음 재생
        {
            audioSources[1].pitch = pitch;
            audioSources[1].PlayOneShot(audioDictionary[name]);
        }
    }

    
}
