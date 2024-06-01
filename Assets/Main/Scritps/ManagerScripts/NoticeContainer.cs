using UnityEngine;
using UnityEngine.Events;

public class NoticeContainer : MonoBehaviour
{
    public Notice[] notices;
    public UnityEvent _event;

    public void StartNotice()
    {
        GameManager.Instance.guideManager.SetNotice(notices, _event);

    }
}
