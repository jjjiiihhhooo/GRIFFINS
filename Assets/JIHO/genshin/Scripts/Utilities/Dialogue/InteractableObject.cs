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
        if (!ready) return;
        Debug.Log("interact");
        actionOnInteract?.Invoke();

        //for (int i = 0; i < eventNames.Length; i++)
        //{
        //    GameManager.Instance.event_dictionary[eventNames[i]]?.Invoke();
        //}

        if (oneTime)
        {
            Player.Instance.SetActiveInteraction(false);
            GetComponent<Collider>().enabled = false;
            actionOnInteract = null;
        }
    }

    public void F_TO_Quest()
    {
        GameManager.Instance.questManager.InputQuestCheck(KeyCode.F);
        if (oneTime)
            Destroy(this.gameObject);
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

