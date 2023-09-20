using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public void PinballAnim()
    {
        PlayerController.Instance.isPinball = true;
        PlayerController.Instance.rigid.useGravity = false;
        PlayerController.Instance.col.enabled = false;
        PlayerController.Instance.playerCharacter.forward = PlayerController.Instance.cam.transform.forward;

        if (PlayerController.Instance.attackerEffect != null)
        {
            GameObject Effect = Instantiate(PlayerController.Instance.attackerEffect, transform.position, Quaternion.identity);
        }
    }
}
