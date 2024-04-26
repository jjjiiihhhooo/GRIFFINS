using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlySingleton : MonoBehaviour
{
    public static OnlySingleton Instance;

    public CinemachineShake camShake;
    public Transform normalTransform;
    public CinemachineVirtualCamera red_E_cam;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void BackToNormal()
    {
        camShake.transform.position = normalTransform.position;
    }
}
