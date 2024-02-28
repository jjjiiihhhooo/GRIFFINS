using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;



    public void SetTimeline()
    {
        GameManager.Instance.timelineManager.TimelineChange(director);
    }

    public void TimelineStart()
    {
        GameManager.Instance.timelineManager.TimelineStart();
    }

    public void TimelineEnd()
    {
        Debug.Log("timelineEndSignal");
        GameManager.Instance.timelineManager.TimelineEnd();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SetTimeline();
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            TimelineStart();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            TimelineEnd();
        }
    }
}
