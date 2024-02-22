using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent enter_event;
    [SerializeField] private UnityEvent exit_event;
    [SerializeField] private string physicsName;
    [SerializeField] private string event_key = "";

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == physicsName)
        {
            if(event_key != "") GameManager.Instance.event_dictionary[event_key].Invoke();
            enter_event.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == physicsName)
        {
            if (event_key != "") GameManager.Instance.event_dictionary[event_key].Invoke();
            exit_event.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == physicsName)
        {
            if (event_key != "") GameManager.Instance.event_dictionary[event_key].Invoke();
            enter_event.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == physicsName)
        {
            if (event_key != "") GameManager.Instance.event_dictionary[event_key].Invoke();
            exit_event.Invoke();
        }
    }

    public void PlayerSpawn()
    {
        Player.Instance.PlayerSpawn();
    }
}
