using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent enter_event;
    [SerializeField] private UnityEvent exit_event;
    [SerializeField] private string physicsName;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == physicsName)
        {
            enter_event.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == physicsName)
        {
            exit_event.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == physicsName)
        {
            enter_event.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == physicsName)
        {
            exit_event.Invoke();
        }
    }

    public void PlayerSpawn()
    {
        Player.Instance.PlayerSpawn();
    }
}
