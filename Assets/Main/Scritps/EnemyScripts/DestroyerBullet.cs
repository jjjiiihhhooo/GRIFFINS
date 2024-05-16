using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerBullet : MonoBehaviour
{
    [SerializeField] private float damage;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().GetDamage(damage);
        }
    }
}
