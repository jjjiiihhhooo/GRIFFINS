using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    public float size;
    public float plus;

    private void Awake()
    {
        StartCoroutine(CreateCor());
    }

    private IEnumerator CreateCor()
    {
        Vector3 scale = Vector3.zero;
        while(transform.localScale.x < size)
        {
            yield return new WaitForEndOfFrame();
            scale.x += plus;
            scale.y += plus;
            scale.z += plus;
            transform.localScale = scale;
        }
        GetComponent<Collider>().enabled = true;
    }
}
