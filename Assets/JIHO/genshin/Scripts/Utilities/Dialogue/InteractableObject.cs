using UnityEngine;
using UnityEngine.Events;



public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string interactorName;
    [SerializeField] private UnityEvent actionOnInteract;
    [SerializeField] private string[] eventNames;


    [SerializeField] private bool oneTime;
    [SerializeField] private bool ready = true;

    public string InteractorName { get { return interactorName; } }


    public void OnInteract()
    {
        Debug.Log("interact");
        actionOnInteract?.Invoke();

        for (int i = 0; i < eventNames.Length; i++)
        {
            GameManager.Instance.event_dictionary[eventNames[i]]?.Invoke();
        }

        if (oneTime)
        {
            GetComponent<Collider>().enabled = false;
            Player.Instance.SetActiveInteraction(false);
            actionOnInteract = null;
        }
    }

    public bool GetReady()
    {
        return ready;
    }

    public void SetReady(bool _bool)
    {
        ready = _bool;
    }
}

