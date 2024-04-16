
using UnityEngine;
using UnityEngine.UI;

public class SkillData : MonoBehaviour
{

    public bool isHand; //잡았는지
    public GameObject handObj;
    public CameraZoom camZoom;
    public Transform catchTransform;
    public Transform lookatTransform;
    public bool isGHand;


    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    public float distanceFromPoint;

    public float grapplingCd;
    public float grapplingCdTimer;
    public float grappleSpeed;

    public bool grappling;
    public bool touch;
    public bool isSprint;

    public LayerMask grappleMask;
    public LineRenderer lr;

    public LineRenderer Glr;

    public Vector3 grapplePoint;
    public Vector3 velocity;
    public Vector3 Gvelocity;

    public SpringJoint Gjoint;
    public GameObject GhandObj;
    public Transform shoot_obj;
    public Transform Gshoot_obj;


    private void Start()
    {
        if (camZoom == null) camZoom = FindObjectOfType<CameraZoom>();
    }

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
