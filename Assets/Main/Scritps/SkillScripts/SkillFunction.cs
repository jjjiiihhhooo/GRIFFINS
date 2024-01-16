using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFunction : MonoBehaviour
{

    public Transform catchTransform; //����� �� ��ġ
    public Transform lookatTransform; //����� �� ī�޶� �ٶ� ��ġ
    public GameObject handObj; //���� ������Ʈ

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
