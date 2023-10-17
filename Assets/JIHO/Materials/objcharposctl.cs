using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objcharposctl : MonoBehaviour
{
    public Rigidbody rbchar;


    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Player") rbchar.AddForce(rbchar.velocity * 0.2f, ForceMode.Impulse);


    }

}
