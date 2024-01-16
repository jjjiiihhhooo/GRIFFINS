using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFunction : MonoBehaviour
{

    public Transform catchTransform; //잡았을 때 위치
    public Transform lookatTransform; //잡았을 때 카메라가 바라볼 위치
    public GameObject handObj; //잡은 오브젝트

    public Transform cam;
    public Transform shoot_obj;
    public LayerMask grappleMask;
    public LineRenderer lr;

    public float maxGrappleDistance;
    public float grappleDelayTime;

    public Vector3 grapplePoint;

    public float grapplingCd;
    public float grapplingCdTimer;

    public KeyCode grappleKey = KeyCode.E;

    public bool grappling;

    public bool isSprint;

    private void Update()
    {
        if (grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling) lr.SetPosition(0, shoot_obj.position);
    }

}
