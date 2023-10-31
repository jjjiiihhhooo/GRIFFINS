using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectCall : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private UnityEvent _event;

    private void OnEnable()
    {
        _event.Invoke();
    }

    public void ChangeEffectMasaage()
    {
        StartCoroutine(changeEffectCor());
    }

    private IEnumerator changeEffectCor()
    {
        
        float curTime = time;
        while(curTime > 0)
        {
            transform.forward = Camera.main.transform.position - transform.position;
            yield return new WaitForEndOfFrame();
            curTime -= Time.deltaTime;
        }

        this.gameObject.SetActive(false);
    }

}
