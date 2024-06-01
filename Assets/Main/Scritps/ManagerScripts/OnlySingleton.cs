using Cinemachine;
using UnityEngine;

public class OnlySingleton : MonoBehaviour
{
    public static OnlySingleton Instance;

    public CinemachineShake camShake;
    public Transform normalTransform;
    public CinemachineVirtualCamera red_E_cam;
    public CinemachineVirtualCamera green_E_cam;
    public CinemachineVirtualCamera white_Q_cam;
    public Transform miniMapCam;
    public Transform mainCam;
    public Transform destinationTransform;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        destinationTransform.position = mainCam.position;
    }

    public void BackToNormal()
    {
        camShake.transform.position = normalTransform.position;
    }
}
