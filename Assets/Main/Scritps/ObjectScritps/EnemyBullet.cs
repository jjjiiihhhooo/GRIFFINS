using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public Vector3 dir;
    private Rigidbody rigid;

    [SerializeField] private float speed;
    private void OnEnable()
    {
        transform.forward = dir;
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Environment")
        {
            Destroy(this.gameObject);
        }

        if (collider.tag == "Player")
        {
            collider.GetComponent<Player>().GetDamage(1);
            Destroy(this.gameObject);
        }
        
    }
}
