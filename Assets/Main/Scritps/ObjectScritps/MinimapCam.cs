using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{

    private Player player;
    private Vector3 pos;

    private void Update()
    {
        CamMove();
    }

    private void CamMove()
    {
        if (Player.Instance == null) return;
        if (player == null) player = Player.Instance;

        pos.x = player.transform.position.x;
        pos.z = player.transform.position.z;
        pos.y = this.transform.position.y;

        transform.position = pos;
    }
}
