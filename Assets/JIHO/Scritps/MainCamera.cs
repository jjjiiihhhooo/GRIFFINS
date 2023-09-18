using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 position;

    public Transform cameraArm;

    private void LookAt()
    {
        if(!player.IsPinball)
        {
            Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;

            float x = camAngle.x - mousePos.y;

            if (x < 180f) x = Mathf.Clamp(x, -1f, 70f);
            else x = Mathf.Clamp(x, 335f, 361f);

            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mousePos.x, camAngle.z);
        }    

    }

    private void Update()
    {
        LookAt();
    }

}
