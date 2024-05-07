using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;



    public void SetTimeline()
    {
       // GameManager.Instance.timelineManager.TimelineChange(director);
    }

    public void TimelineStart()
    {
        SetTimeline();
        //GameManager.Instance.timelineManager.TimelineStart();
    }

    public void PauseToStart()
    {
       // GameManager.Instance.timelineManager.TimelinePauseToStart();
    }

    public void TimelineEnd()
    {
        Debug.Log("timelineEndSignal");
        //GameManager.Instance.timelineManager.TimelineEnd();
    }
}
