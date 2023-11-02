using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        StopCoroutine(AttackActiveCor());
        StartCoroutine(AttackActiveCor());
    }

    private IEnumerator AttackActiveCor()
    {
        float curTime = time;
        while (curTime > 0)
        {
            yield return new WaitForEndOfFrame();
            curTime -= Time.deltaTime;
        }

        this.gameObject.SetActive(false);
    }
}
