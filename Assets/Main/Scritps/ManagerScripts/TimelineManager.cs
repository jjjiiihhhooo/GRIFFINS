
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : SerializedMonoBehaviour
{
    [SerializeField] private PlayableAsset playableAsset;
    [SerializeField] private PlayableDirector playableDirecter;

    [SerializeField] private Dictionary<string, PlayableAsset> playableAssetDic;

    public void Init()
    {
        playableDirecter = GetComponent<PlayableDirector>();

    }

    public void TimelineStart()
    {
        playableDirecter.Play();
    }

    public void TimelineChange(string name)
    {
        playableDirecter.playableAsset = playableAssetDic[name];
    }

}
