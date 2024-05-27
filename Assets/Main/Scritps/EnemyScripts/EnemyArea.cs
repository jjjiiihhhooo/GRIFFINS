using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    public float time;

    private void Update()
    {
        if (time > 0) time -= Time.deltaTime;
        else
        {
            Instantiate(obj, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


}
