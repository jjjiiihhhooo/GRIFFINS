using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : SerializedMonoBehaviour
{
    public Enemy enemy;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("attackCol"))
        {
            enemy.GetDamage(PlayerController.Instance.currentUnit.curDamage);
        }
    }
}
