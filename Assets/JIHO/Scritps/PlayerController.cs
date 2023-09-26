using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public State<PlayerController> currentState;

    [SerializeField] private float currentHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float rayDistance;

    [SerializeField] private float mouseSpeed;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    

    [SerializeField] private float scopeForce;
    [SerializeField] private float tempForce;

    [SerializeField] private int pinballCount;
    [SerializeField] private int pinballMaxCount;

    [SerializeField] private bool isJump;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isRotate;

    [SerializeField] private bool isMouse;

    [SerializeField] private Vector3 moveVec = Vector3.zero;
    [SerializeField] private Vector3 PinBallLandingPos = Vector3.zero;
    [SerializeField] private Vector3[] routeVectors;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform firePos;
    [SerializeField] private Transform playerCharacter;
    [SerializeField] private LineRenderer line;

    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject scope;

    private float hRot;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;

    private StarterAssetsInputs _input;
    private PlayerInput _playerInput;

    public bool LockCameraPosition = false;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }


    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;

    public bool isPinball;
    public GameObject CinemachineCameraTarget;

    public Rigidbody rigid;
    public CapsuleCollider col;
    public MainCamera cam;

    public Animator animator;
    public Animator attackerAnimator;

    private const float _threshold = 0.01f;

    public bool IsMouse { get => isMouse; }
    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsRotate { get => isRotate; set => isRotate = value; }
    public bool IsPinball { get => isPinball; }
    public bool IsJump { get => isJump; set => isJump = value; }

    public float HRot { get => hRot; set => hRot = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    
    public Vector3 MoveVec { get => moveVec; set => moveVec = value; }

    public PlayerIdleState PlayerIdleState { get => playerIdleState; }
    public PlayerWalkState PlayerWalkState { get => playerWalkState; }

    public GameObject attackerEffect;
    public GameObject pinBallMoveEffect;

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

        Init();
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Init()
    {
        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();

        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
        currentState = playerIdleState;
    }


    private void FixedUpdate()
    {
        currentState.StateUpdate(this);
    }

    public void WalkEnter()
    {
        animator.SetBool("Walk", true);
        isMove = true;
    }

    public void Walk()
    {

        //camForward = Camera.main.transform.forward;
        //camForward.y = 0;
        //camForward.Normalize();

        //camForward = camForward - moveVec;

        //float rotAngle = Mathf.Atan2(camForward.z, camForward.x) * Mathf.Rad2Deg * -2;

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotAngle, 0), Time.fixedDeltaTime * mouseSpeed);

        //rigid.velocity = transform.forward * moveSpeed;

        //if (isRotate)
        //{

        //    //float rotAngle = hRot * mouseSpeed * Time.fixedDeltaTime;
        //    //rigid.rotation = Quaternion.AngleAxis(rotAngle, Vector3.up) * rigid.rotation;
        //}

        //if (isJump)
        //{
        //    rigid.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.VelocityChange);
        //    isJump = false;
        //}

        //moveVec.x = Input.GetAxisRaw("Horizontal");
        //moveVec.z = Input.GetAxisRaw("Vertical");


        //rigid.velocity = moveVec * currentSpeed;

        //isMove = moveVec.magnitude != 0;

        //Vector3 lookForward = new Vector3(cam.cameraArm.forward.x, 0f, cam.cameraArm.forward.z).normalized;
        //Vector3 lookRight = new Vector3(cam.cameraArm.right.x, 0f, cam.cameraArm.right.z).normalized;
        //Vector3 moveDir = lookForward * moveVec.y + lookRight * moveVec.x;

        //playerCharacter.forward = lookForward;

        //if (isMove) transform.position += moveDir * Time.deltaTime * CurrentSpeed;
    }


    public void IdleEnter()
    {
        animator.SetBool("Walk", false);
        isMove = false;
    }

    public void Idle()
    {
        //if (isRotate)
        //{
        //    float rotAngle = hRot * mouseSpeed * Time.fixedDeltaTime;
        //    rigid.rotation = Quaternion.AngleAxis(rotAngle, Vector3.up) * rigid.rotation;
        //}

        //if (isJump)
        //{
        //    rigid.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.VelocityChange);
        //    isJump = false;
        //}
    }


    public void PinBallReadyEnter()
    {
        animator.SetBool("Ready", true);
        attackerAnimator.gameObject.SetActive(true);
        line.gameObject.SetActive(true);
    }

    public void PinBallReadyUpdate()
    {
        PinBallLandingPos = playerCharacter.position;
        Debug.Log("1" + PinBallLandingPos);
        Vector3 routeVec = cam.mousePos - PinBallLandingPos;

        for (int i = 0; i < pinballMaxCount; i++)
        {
            if (Physics.Raycast(PinBallLandingPos, routeVec, out hit, Mathf.Infinity, layer))
            {
                line.SetPosition(i, PinBallLandingPos);
                Vector3 incomingVector = routeVec;
                incomingVector = incomingVector.normalized;
                Vector3 normalVector = hit.normal;
                Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
                reflectVector = reflectVector.normalized;
                routeVec = reflectVector;
                PinBallLandingPos = hit.point;
                routeVectors[i] = hit.point;
            }
        }
    }

    public void PinBallReadyExit()
    {
        attackerAnimator.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
    }

    public void PinBallReadyCancel()
    {
        animator.SetBool("Ready", false);
        attackerAnimator.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
        ChangeState(playerIdleState);
    }

    public void PinBallEnter()
    {
        StartCoroutine(PinBallStartCor());
    }

    public void PinBallUpdate()
    {
        //if (!isPinball) return;
        //Debug.Log("ÇÉº¼ ½ÃÀÛ");
        //pinBallMoveEffect.SetActive(true);
        //attackerAnimator.gameObject.SetActive(false);
        
        
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PinBallMove")) animator.SetTrigger("Dash");

        //if (pinballCount >= pinballMaxCount)
        //{
            
        //    return;
        //}

        //transform.position = Vector3.MoveTowards(transform.position, routeVectors[pinballCount], Time.deltaTime * force);

        //if(Physics.Raycast(playerCharacter.transform.position, routeVectors[pinballCount] - transform.position, out hit, Mathf.Infinity, enemyLayer))
        //{
        //    if (Vector3.Distance(transform.position, hit.point) < 1f)
        //    {
        //        GameObject temp = Instantiate(hit.transform.GetComponent<Enemy>().damageEffect, transform.position, Quaternion.identity);
        //        //hit.transform.GetComponent<Enemy>().anim.SetTrigger("Hit");
        //    }
        //}

        //if (Physics.Raycast(playerCharacter.transform.position, routeVectors[pinballCount] - transform.position, out hit, Mathf.Infinity, layer))
        //{
        //    if (Vector3.Distance(transform.position, hit.point) < 1f) if (hit.transform.gameObject.CompareTag("item")) hit.transform.GetComponent<PlatformObj>().anim.SetTrigger("Hit");

        //}

        //playerCharacter.forward = routeVectors[pinballCount] - transform.position;



        //if (Vector3.Distance(transform.position, routeVectors[pinballCount]) < 0.1f) 
        //{
        //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PinBallLanding")) 
        //        animator.SetTrigger("Landing");

            
        //    pinballCount++;
        //    GameObject temp = Instantiate(attackEffect, transform.position, Quaternion.identity);

            

        //    temp.SetActive(true);
        //}

    }

    public void PinBallExit()
    {
        pinBallMoveEffect.SetActive(false);
        animator.SetBool("Ready", false);
        animator.SetTrigger("Exit");
        isPinball = false;
        col.enabled = true;
        pinballCount = 0;
        rigid.useGravity = true;
    }

    private IEnumerator PinBallStartCor()
    {
        attackerAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        isPinball = true;
        rigid.useGravity = false;
        col.enabled = false;
        GameObject Effect = Instantiate(attackerEffect, attackerAnimator.transform.position, Quaternion.identity);
        playerCharacter.forward = cam.transform.forward;
    }

    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }
}
