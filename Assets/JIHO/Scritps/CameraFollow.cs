using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook camFollow;

    public bool isInteraction;
    private void Start()
    {
        isInteraction = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        isInteraction = false;

        Vector3 pPos = PlayerController.Instance.transform.position;
        Vector3 newVec = pPos + new Vector3(0, 5, -10);
        Quaternion newRot = Quaternion.LookRotation(pPos - newVec, Vector3.up);
        SetCameraPosAndRotation(newVec, newRot);
    }


    public void SetCameraPosAndRotation(Vector3 p, Quaternion r)
    {
        if (camFollow != null)
        {
            camFollow.transform.position = p;
            camFollow.transform.rotation = r;
            camFollow.Follow = PlayerController.Instance.transform;
        }
    }

    private void Update()
    {
        if (camFollow == null) return;
        if (!isInteraction)
        {
            camFollow.enabled = true;
        }
        else
        {
            camFollow.enabled = false;

        }

    }

    public void SettingCameraEnable(bool _check)
    {
        isInteraction = _check;
    }

    private void LateUpdate()
    {

    }
}
