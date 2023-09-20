using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject damageEffect;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            GameObject temp = Instantiate(damageEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
