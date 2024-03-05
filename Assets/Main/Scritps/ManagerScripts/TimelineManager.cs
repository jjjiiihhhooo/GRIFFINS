
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : SerializedMonoBehaviour
{
    [SerializeField] private PlayableAsset playableAsset;
    [SerializeField] private PlayableDirector playableDirecter;

    public void Init()
    {
        

    }

    public void TimelineStart()
    {
        Player.Instance.gameObject.SetActive(false);
        //OnlySingleton.Instance.gameObject.SetActive(false);
        playableDirecter.gameObject.SetActive(true);
        playableDirecter.Play();
    }

    public void TimelineEnd()
    {
        Debug.Log("timelineEnd");
        playableDirecter.Stop();
        Player.Instance.gameObject.SetActive(true);
        //OnlySingleton.Instance.gameObject.SetActive(true);
        playableDirecter.gameObject.SetActive(false);
    }

    public void TimelinePauseToStart()
    {
        playableDirecter.Play();
    }

    public void TimelineChange(PlayableDirector timeline)
    {
        playableDirecter = timeline;
    }

}
