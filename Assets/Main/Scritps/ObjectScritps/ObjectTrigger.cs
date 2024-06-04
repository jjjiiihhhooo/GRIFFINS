
using UnityEngine;
using UnityEngine.Events;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent enter_event;
    [SerializeField] private UnityEvent exit_event;
    [SerializeField] private bool oneTime;
    [SerializeField] private string physicsName;
    //[SerializeField] private string event_key = "";
    //[SerializeField] private string exit_event_key = "";

    public Transform target;
    public GameObject destroyObj;

    public bool fade;
    public bool onlyObj;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.name == physicsName)
        {
            //if (event_key != "") GameManager.Instance.event_dictionary[event_key]?.Invoke();
            enter_event?.Invoke();
            PlayerTrigger();
            if (oneTime) GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.name == physicsName)
        {
            //if (exit_event_key != "") GameManager.Instance.event_dictionary[exit_event_key]?.Invoke();
            //exit_event?.Invoke();

            if (oneTime) GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == physicsName)
        {
            //if (event_key != "") GameManager.Instance.event_dictionary[event_key]?.Invoke();
            enter_event?.Invoke();

            if (oneTime) GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name == physicsName)
        {
            //if (exit_event_key != "") GameManager.Instance.event_dictionary[exit_event_key]?.Invoke();
            exit_event?.Invoke();

            if (oneTime) GetComponent<Collider>().enabled = false;
        }
    }

    public void PlayerSpawn()
    {
        Player.Instance.PlayerSpawn();
    }

    public void PlayerTrigger()
    {
        Player.Instance.curTrigger = this;
    }

    public void WarningMessage()
    {
        GameManager.Instance.guideManager.WarningSetMessage();
    }

    public void EnemyWave(int index)
    {
        GameManager.Instance.enemyManager.SpawnWave(index);
    }

    public void PlayerToTransform()
    {
        if (fade) GameManager.Instance.uiManager.FadeInOut();

        if(target != null)
            Player.Instance.transform.position = target.position;

        if (destroyObj != null) 
        { 
            Destroy(destroyObj);
            
            if(!onlyObj)
            {
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
