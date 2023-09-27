using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
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
    [SerializeField] private float speed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCurSpeed;
    [SerializeField] private float dashCurTime;
    [SerializeField] private float dashMinus;
    [SerializeField] private float dashTime;
    [SerializeField] private float speedChangeRate = 10.0f;
    [SerializeField] private float rotationSmoothTime = 0.12f;

    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private float groundedRadius = 0.28f;

    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -15.0f;
    [SerializeField] private float jumpTimeout = 0.20f;
    [SerializeField] private float fallTimeout = 0.15f;

    [SerializeField] private float scopeForce;
    [SerializeField] private float tempForce;

    [SerializeField] private bool isJump;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isRotate;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isMouse;
    [SerializeField] private bool isBounce;
    [SerializeField] private bool grounded = true;
    [SerializeField] private bool interaction;

    [SerializeField] private Vector3 moveVec = Vector3.zero;
    [SerializeField] private Vector3 dashPos = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform playerCharacter;

    [SerializeField] private GameObject attackEffect;
    public TextMeshProUGUI textText;

    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 100.0f;
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float hRot;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private bool hasAnimator;


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

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;

    private StarterAssetsInputs _input;
    private PlayerInput _playerInput;
    private CharacterController characterController;

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;

    public GameObject CinemachineCameraTarget;

    public GameObject cam;

    public Animator animator;
    public Animator attackerAnimator;

    private const float _threshold = 0.01f;

    public bool IsMouse { get => isMouse; }
    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsRotate { get => isRotate; set => isRotate = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public bool IsDash { get => isDash; }

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
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Init()
    {
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera");
        }
        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();
        characterController = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
        currentState = playerIdleState;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Walk();
        Debug.Log(verticalVelocity);
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
        float targetSpeed;
        
        if (isDash)
        {
            WallCheck();

            characterController.Move(transform.forward * (dashCurSpeed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
            dashCurSpeed -= dashMinus / 2;
            //if(dashMinus > 0) dashMinus -= Time.deltaTime;
            dashCurTime -= Time.deltaTime;
            if (dashCurSpeed < moveSpeed || dashCurTime < 0)
            {
                dashCurTime = dashTime;
                dashCurSpeed = dashSpeed;
                dashMinus = 5f;
                isDash = false;
            }
        }
        else
        {
            targetSpeed = _input.sprint ? sprintSpeed : moveSpeed;

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;
            float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  cam.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }
        

        //if (hasAnimator)
        //{
        //    animator.SetFloat(_animIDSpeed, _animationBlend);
        //    animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        //}
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - groundedOffset,
            playerCharacter.transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
            QueryTriggerInteraction.Ignore);

        //if (_hasAnimator)
        //{
        //    _animator.SetBool(_animIDGrounded, Grounded);
        //}
    }

    private void JumpAndGravity()
    {
        if (grounded || interaction)
        {
            fallTimeoutDelta = fallTimeout;

            //if (_hasAnimator)
            //{
            //    _animator.SetBool(_animIDJump, false);
            //    _animator.SetBool(_animIDFreeFall, false);
            //}

            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (_input.jump && jumpTimeoutDelta <= 0.0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (interaction) interaction = false;
                //if (_hasAnimator)
                //{
                //    _animator.SetBool(_animIDJump, true);
                //}
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                //if (_hasAnimator)
                //{
                //    _animator.SetBool(_animIDFreeFall, true);
                //}
            }

            _input.jump = false;
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    public void IdleEnter()
    {
        animator.SetBool("Walk", false);
        isMove = false;
    }

    public void Idle()
    {

    }

    public void Dash()
    {
        if (isDash) return;
        transform.forward = new Vector3(cam.transform.forward.x, transform.forward.y, cam.transform.forward.z);
        dashCurSpeed = dashSpeed;
        dashCurTime = dashTime;
        dashMinus = 5f;
        isDash = true;
        //characterController.Move(transform.forward * 3f);
        //StartCoroutine(dashCor());
    }

    public void WallCheck()
    {
        if (Physics.Raycast(playerCharacter.transform.position, playerCharacter.transform.forward, out hit, 2f, layer))
        {
            Debug.LogError("dd");
            textText.text = "Rebound";
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            interaction = true;
            isBounce = true;
            Vector3 incomingVector = hit.point - playerCharacter.position;
            incomingVector = incomingVector.normalized;
            Vector3 normalVector = hit.normal;
            Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
            reflectVector = reflectVector.normalized;
            transform.forward = reflectVector;
            dashCurSpeed = dashCurSpeed / 1.3f;
            
        }
    }

   
    //public void PinBallReadyEnter()
    //{
    //    animator.SetBool("Ready", true);
    //    attackerAnimator.gameObject.SetActive(true);
    //    line.gameObject.SetActive(true);
    //}

    //public void PinBallReadyUpdate()
    //{
    //    PinBallLandingPos = playerCharacter.position;
    //    Debug.Log("1" + PinBallLandingPos);
    //    Vector3 routeVec = cam.mousePos - PinBallLandingPos;

    //    for (int i = 0; i < pinballMaxCount; i++)
    //    {
    //        if (Physics.Raycast(PinBallLandingPos, routeVec, out hit, Mathf.Infinity, layer))
    //        {
    //            line.SetPosition(i, PinBallLandingPos);
    //            Vector3 incomingVector = routeVec;
    //            incomingVector = incomingVector.normalized;
    //            Vector3 normalVector = hit.normal;
    //            Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
    //            reflectVector = reflectVector.normalized;
    //            routeVec = reflectVector;
    //            PinBallLandingPos = hit.point;
    //            routeVectors[i] = hit.point;
    //        }
    //    }
    //}

    //public void PinBallReadyExit()
    //{
    //    attackerAnimator.gameObject.SetActive(false);
    //    line.gameObject.SetActive(false);
    //}

    //public void PinBallReadyCancel()
    //{
    //    animator.SetBool("Ready", false);
    //    attackerAnimator.gameObject.SetActive(false);
    //    line.gameObject.SetActive(false);
    //    ChangeState(playerIdleState);
    //}

    //public void PinBallEnter()
    //{
    //    StartCoroutine(PinBallStartCor());
    //}

    //public void PinBallUpdate()
    //{
    //    //if (!isPinball) return;
    //    //Debug.Log("ÇÉº¼ ½ÃÀÛ");
    //    //pinBallMoveEffect.SetActive(true);
    //    //attackerAnimator.gameObject.SetActive(false);
        
        
    //    //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PinBallMove")) animator.SetTrigger("Dash");

    //    //if (pinballCount >= pinballMaxCount)
    //    //{
            
    //    //    return;
    //    //}

    //    //transform.position = Vector3.MoveTowards(transform.position, routeVectors[pinballCount], Time.deltaTime * force);

    //    //if(Physics.Raycast(playerCharacter.transform.position, routeVectors[pinballCount] - transform.position, out hit, Mathf.Infinity, enemyLayer))
    //    //{
    //    //    if (Vector3.Distance(transform.position, hit.point) < 1f)
    //    //    {
    //    //        GameObject temp = Instantiate(hit.transform.GetComponent<Enemy>().damageEffect, transform.position, Quaternion.identity);
    //    //        //hit.transform.GetComponent<Enemy>().anim.SetTrigger("Hit");
    //    //    }
    //    //}

    //    //if (Physics.Raycast(playerCharacter.transform.position, routeVectors[pinballCount] - transform.position, out hit, Mathf.Infinity, layer))
    //    //{
    //    //    if (Vector3.Distance(transform.position, hit.point) < 1f) if (hit.transform.gameObject.CompareTag("item")) hit.transform.GetComponent<PlatformObj>().anim.SetTrigger("Hit");

    //    //}

    //    //playerCharacter.forward = routeVectors[pinballCount] - transform.position;



    //    //if (Vector3.Distance(transform.position, routeVectors[pinballCount]) < 0.1f) 
    //    //{
    //    //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PinBallLanding")) 
    //    //        animator.SetTrigger("Landing");

            
    //    //    pinballCount++;
    //    //    GameObject temp = Instantiate(attackEffect, transform.position, Quaternion.identity);

            

    //    //    temp.SetActive(true);
    //    //}

    //}

    //public void PinBallExit()
    //{
    //    pinBallMoveEffect.SetActive(false);
    //    animator.SetBool("Ready", false);
    //    animator.SetTrigger("Exit");
    //    isPinball = false;
    //    pinballCount = 0;
    //}

    //private IEnumerator PinBallStartCor()
    //{
    //    attackerAnimator.SetTrigger("Attack");
    //    yield return new WaitForSeconds(0.5f);
    //    isPinball = true;
    //    GameObject Effect = Instantiate(attackerEffect, attackerAnimator.transform.position, Quaternion.identity);
    //    playerCharacter.forward = cam.transform.forward;
    //}

    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }
}
