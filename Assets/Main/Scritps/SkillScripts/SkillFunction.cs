using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFunction : MonoBehaviour
{

    public Transform catchTransform; //잡았을 때 위치
    public Transform lookatTransform; //잡았을 때 카메라가 바라볼 위치
    public GameObject handObj; //잡은 오브젝트
    public GameObject GhandObj;

    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    public Transform shoot_obj;
    public Transform Gshoot_obj;

    [Header("Cooldown")]
    public float grapplingCd;
    public float grapplingCdTimer;

    public LayerMask grappleMask;
    public LineRenderer lr;

    public LineRenderer Glr;

    public Vector3 grapplePoint;
    public Vector3 velocity;
    public Vector3 Gvelocity;

    public SpringJoint Gjoint;
    public float distanceFromPoint;


    public bool grappling;
    public bool touch;
    public bool isSprint;

    private void Update()
    {
        if (grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling) lr.SetPosition(0, shoot_obj.position);
        if (Player.Instance.skillData.isGHand) 
        {
            Glr.SetPosition(0, Gshoot_obj.position); 
            Glr.SetPosition(1, GhandObj.transform.position);
            Gjoint.connectedAnchor = transform.position;
        }
    }

}
