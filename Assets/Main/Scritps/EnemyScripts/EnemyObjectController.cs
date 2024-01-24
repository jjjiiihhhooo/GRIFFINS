using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectController : SerializedMonoBehaviour
{
    public EnemyAttackObject attackObject;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().testHp--;
        }

        if(!collision.transform.CompareTag(this.transform.tag) && !this.transform.CompareTag("usingObject"))
        {
            Destroy(this.gameObject);
        }
    }


}
