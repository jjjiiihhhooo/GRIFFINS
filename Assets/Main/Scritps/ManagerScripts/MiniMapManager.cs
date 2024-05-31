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

}
