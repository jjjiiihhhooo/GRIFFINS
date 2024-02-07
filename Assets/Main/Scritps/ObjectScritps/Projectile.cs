using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rigid;
    public float speed = 30f;
    public bool isRed;

    private bool isCol;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
        
    }

    private void Start()
    {
        rigid.velocity = transform.forward * speed;
    }

    private void Update()
    {
        transform.GetChild(0).transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!(other.tag == "useObject" || other.tag == "usingObject"))
        {
            if (isCol) return;

            isCol = true;
            if (Player.Instance.currentCharacter.index == 3)
            {
                Rigidbody temp = this.transform.GetChild(0).GetComponent<Rigidbody>();
                Destroy(temp);
                transform.GetChild(0).gameObject.layer = 11;
                transform.GetChild(0).transform.SetParent(null);
                Destroy(this.gameObject);
            }
            else
            {
                Rigidbody rigid = transform.GetChild(0).GetComponent<Rigidbody>();
                transform.GetChild(0).transform.SetParent(null);
                rigid.velocity = this.rigid.velocity;
                rigid.useGravity = true;
                Destroy(this.gameObject);
            }
        }

    }
}
