using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public PlayerMovementGrappling pm;

    [Header("Swinging")]
    public float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.C;

    public bool swinging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //    if (Input.GetKeyDown(swingKey)) StartSwing();
        //    if (Input.GetKeyUp(swingKey)) StopSwing();
        if (Player.Instance.currentCharacter.isGrappleReady) return;

        CheckForSwingPoints();

        if (joint != null) OdmGearMovement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;
        if (Player.Instance.currentCharacter.GetType() != typeof(GreenCharacter)) return;
        RaycastHit sphereCastHit;
        Physics.SphereCast(Camera.main.transform.position, predictionSphereCastRadius, Camera.main.transform.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        // Option 2 - Indirect (predicted) Hit
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        // Option 3 - Miss
        else
            realHitPoint = Vector3.zero;

        //if (predictionPoint == null) predictionPoint = GameManager.Instance.predictionHitTransform;

        // realHitPoint found
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        //// realHitPoint not found
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    public void StartSwing()
    {
        Player playerController = transform.GetComponent<Player>();

        if (playerController.isGround) return;
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);

        if (playerController.movementStateMachine.CurStateName() != "PlayerFallingState")
        {
            playerController.movementStateMachine.ChangeState(playerController.movementStateMachine.FallingState);
        }
        lr.gameObject.SetActive(true);
        Player.Instance.currentCharacter.StopGrapple();

        // deactivate active grapple
        //if (GetComponent<Grappling>() != null)
        //    GetComponent<Grappling>().StopGrapple();
        //pm.ResetRestrictions();

        //pm.swinging = true;
        swinging = true;
        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;
        playerController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing()
    {
        //pm.swinging = false;
        swinging = false;
        lr.positionCount = 0;
        lr.gameObject.SetActive(false);
        Destroy(joint);
    }

    private void OdmGearMovement()
    {
        // right
        if (Input.GetKey(KeyCode.D)) rb.AddForce(Camera.main.transform.right * horizontalThrustForce * Time.deltaTime);
        // left
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-Camera.main.transform.right * horizontalThrustForce * Time.deltaTime);

        // forward
        if (Input.GetKey(KeyCode.W)) rb.AddForce(Camera.main.transform.forward * horizontalThrustForce * Time.deltaTime);

        // shorten cable
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        // extend cable
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        // if not grappling, don't draw rope
        if (!joint) return;


        transform.rotation = Player.Instance.CameraRecenteringUtility.VirtualCamera.transform.rotation;
        transform.rotation = new Quaternion(0f, Player.Instance.CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, transform.rotation.w);

        currentGrapplePosition =
            Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 20f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
}
