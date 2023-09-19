using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 scopePosition;

    [SerializeField] private LayerMask layer;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private Ray ray;

    [SerializeField] private float rayDistance;
    [SerializeField] private float mouseSpeed;

    [SerializeField] private LineRenderer line;
    [SerializeField] private Camera thisCam;

    public Vector3 rayDir;
    public Vector3 mousePos;

    public Transform cameraArm;

    private void Update()
    {
        LookAt();
        CheckCamVec();
    }

    private void LookAt()
    {
        if(!player.IsPinball || player.IsScope)
        {
            thisCam.depth = 0;
            Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;

            float x = camAngle.x - mousePos.y;

            if (x < 180f) x = Mathf.Clamp(x, -1f, 70f);
            else x = Mathf.Clamp(x, 335f, 361f);

            if (player.IsScope) transform.localPosition = scopePosition;
            else transform.localPosition = position;

            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mousePos.x, camAngle.z);
        }
        else
        {
            thisCam.depth = -2;
        }

        //if(player.IsScope)
        //{
            
        //    float mouseX = 0;
        //    float mouseY = 0;
        //    mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        //    mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

        //    mouseY = Mathf.Clamp(mouseY, -55.0f, 55.0f);
        //    //if (mouseX >= 360 || mouseX <= -360) mouseX = mouseX % 360;
        //    rotation.x = -mouseY;
        //    rotation.y = mouseX;

        //    this.transform.localEulerAngles = rotation;
        //}
    }

    private void CheckCamVec()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(transform.position, ray.direction, out hit, Mathf.Infinity, layer))
        {
            //line.SetPosition(0, Camera.main.transform.position);
            //line.SetPosition(1, hit.point);
            rayDir = hit.point - Camera.main.transform.position;
            mousePos = hit.point;
        }
    }

}
