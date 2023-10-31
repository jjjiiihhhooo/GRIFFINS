using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] private float time;

    private float curTime;

    private void OnEnable()
    {
        curTime = time;
    }

    private void Update()
    {
        if (curTime > 0) curTime -= Time.deltaTime;
        else gameObject.SetActive(false);
    }
}
