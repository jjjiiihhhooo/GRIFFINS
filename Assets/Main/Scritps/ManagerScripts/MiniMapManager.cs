using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    private Camera miniMapCam;
    private Player player;
    public GameObject temp;
    public GameObject image;

    private Vector3 pos;

    private void Update()
    {
        if (Player.Instance == null) return;
        if (OnlySingleton.Instance == null) return;
        if (player == null) player = Player.Instance;
        if (miniMapCam == null) miniMapCam = OnlySingleton.Instance.miniMapCam.GetComponent<Camera>();

        CamMove();
        //PointMove();
    }

    private void CamMove()
    {
        pos.x = player.transform.position.x;
        pos.z = player.transform.position.z;
        pos.y = miniMapCam.transform.position.y;

        miniMapCam.transform.position = pos;
    }

    private void PointMove()
    {
        if(ObjectisInCamera(player.transform))
        {
            Vector2 pos = miniMapCam.WorldToScreenPoint(player.transform.position);
            if (temp == null) temp = Instantiate(image, GameManager.Instance.uiManager.canvas.gameObject.transform);
            pos.x = pos.x * -1;
            pos.y = pos.y * -1;
            temp.transform.position = pos;
        }
    }

    private bool ObjectisInCamera(Transform target)
    {
        Vector3 screenPoint = miniMapCam.WorldToViewportPoint(target.position);
        bool isIn = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return isIn;
    }
}
