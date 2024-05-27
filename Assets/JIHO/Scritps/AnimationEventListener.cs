using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent[] events;

    public void EventCall(int index)
    {
        if (index >= 0 && index < events.Length)
        {
            events[index].Invoke();
        }
        else
        {
            Debug.Log("index out of range");
        }
    }
}
