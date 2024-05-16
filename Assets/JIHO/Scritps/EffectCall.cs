using System.Collections;
using UnityEngine;

public class EffectCall : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        ChangeEffectMasaage();
    }

    public void ChangeEffectMasaage()
    {
        StopCoroutine(changeEffectCor());
        StartCoroutine(changeEffectCor());
    }

    private IEnumerator changeEffectCor()
    {

        float curTime = time;
        while (curTime > 0)
        {
            transform.forward = Camera.main.transform.position - transform.position;
            yield return new WaitForEndOfFrame();
            curTime -= Time.deltaTime;
        }

        this.gameObject.SetActive(false);
    }

}
