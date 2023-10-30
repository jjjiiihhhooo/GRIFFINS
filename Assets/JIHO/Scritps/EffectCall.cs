using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectCall : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        ChangeEffectMasaage();
    }

    public void ChangeEffectMasaage()
    {
        StartCoroutine(ChangeEffectCor());
    }

    private IEnumerator ChangeEffectCor()
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
