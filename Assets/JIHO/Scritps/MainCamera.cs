using System.IO.Enumeration;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float mouseSpeed = 100f;
    [SerializeField] private float clampAngle = 70f;
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 camNormalDir;
    [SerializeField] private Vector3 camFinalDir;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float finalDistance;
    [SerializeField] private float smoothness = 10f;

    private float rotX;
    private float rotY;

    private void Awake()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        camNormalDir = cam.localPosition.normalized;
        finalDistance = cam.localPosition.magnitude;
    }

    private void LateUpdate()
    {
        InputMouse();
        CamMove();
    }

    private void InputMouse()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * mouseSpeed * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void CamMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, followTransform.position, followSpeed * Time.deltaTime);
        camFinalDir = transform.TransformPoint(camNormalDir * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, camFinalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }

        cam.localPosition = Vector3.Lerp(cam.localPosition, camNormalDir * finalDistance, Time.deltaTime * smoothness);
    }
}

