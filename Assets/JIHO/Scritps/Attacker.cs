using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public void PinballAnim()
    {
        if (PlayerController.Instance.attackerEffect != null)
        {
            GameObject Effect = Instantiate(PlayerController.Instance.attackerEffect, transform.position, Quaternion.identity);
        }
    }
}
