using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private GameObject attackCol_obj;

    private IEnumerator AttackCor()
    {
        attackCol_obj.SetActive(true);

        float curTime = time;
        while (curTime > 0)
        {
            yield return new WaitForEndOfFrame();
            curTime -= Time.deltaTime;
        }

        attackCol_obj.SetActive(false);
    }

}
