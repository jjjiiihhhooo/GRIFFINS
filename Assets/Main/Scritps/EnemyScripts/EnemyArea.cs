using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    [SerializeField] private GameObject[] objs;

    public float time;

    private void Update()
    {
        if (time > 0) time -= Time.deltaTime;
        else
        {
            for(int i = 0; i < objs.Length; i++)
                Instantiate(objs[i], transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }


}
