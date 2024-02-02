using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent _event;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "useObject" || other.tag == "usingObject")
        {
            _event.Invoke();
        }
    }
}
