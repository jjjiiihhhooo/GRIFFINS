using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public State<PlayerController> currentState;

    [SerializeField] private float currentHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashPower;
    [SerializeField] private float rayDistance;

    [SerializeField] private float mouseSpeed;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

    [SerializeField] private float force = 50.0f;

    [SerializeField] private int pinballCount;

    [SerializeField] private bool isAttack;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isReroad;
    [SerializeField] private bool isMouse;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isPinball;

    [SerializeField] private Vector2 moveVec = Vector2.zero;
    [SerializeField] private Vector3 vector = Vector3.zero;
    [SerializeField] private Vector3 camVector = Vector3.zero;
    [SerializeField] private Vector3 playerRotate = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform playerCharacter;

    [SerializeField] private MainCamera cam;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;

    public bool IsMouse { get => isMouse; }
    public bool IsPinball { get => isPinball; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }

    public PlayerIdleState PlayerIdleState { get => playerIdleState; }
    public PlayerWalkState PlayerWalkState { get => playerWalkState; }



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

    private void Init()
    {
        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();

        currentState = playerIdleState;
    }


    private void FixedUpdate()
    {
        Move();
        PinBall();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPinball = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPinball = true;
            rigid.useGravity = false;
            playerCharacter.forward = cam.transform.forward;
        }
    }

    private void Move()
    {
        if (isPinball) return;

        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");

        isMove = moveVec.magnitude != 0;

        Vector3 lookForward = new Vector3(cam.cameraArm.forward.x, 0f, cam.cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cam.cameraArm.right.x, 0f, cam.cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveVec.y + lookRight * moveVec.x;

        playerCharacter.forward = lookForward;
        
        if (isMove) transform.position += moveDir * Time.deltaTime * 5f;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!isPinball) return;
    //    rigid.AddForce(playerCharacter.forward * 0, ForceMode.Impulse);
    //    Vector3 incomingVector = playerCharacter.forward;
    //    incomingVector = incomingVector.normalized;

    //    Vector3 normalVector = collision.contacts[0].normal;

    //    Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
    //    reflectVector = reflectVector.normalized;
    //    playerCharacter.forward = reflectVector;
    //    cam.cameraArm.forward = reflectVector;
    //    rigid.AddForce(reflectVector * force, ForceMode.Impulse);
    //}

    private void PinBall()
    {
        if (!isPinball) return;
        Debug.Log("ÇÉº¼ ½ÃÀÛ");

        

        Debug.DrawRay(playerCharacter.position + playerCharacter.forward * 0.3f, playerCharacter.forward, Color.red);
        if(pinballCount > 10)
        {
            isPinball = false;
            pinballCount = 0;
            rigid.useGravity = true;
            return;
        }
        
       

        if(Physics.Raycast(playerCharacter.position + playerCharacter.forward * 0.3f, playerCharacter.forward, out hit, rayDistance, layer))
        {
            Vector3 incomingVector = playerCharacter.forward;
            incomingVector = incomingVector.normalized;
            Vector3 normalVector = hit.normal;
            Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
            reflectVector = reflectVector.normalized;
            playerCharacter.forward = reflectVector;
            cam.cameraArm.forward = reflectVector;
            pinballCount++;
            Debug.LogError("ÇÉº¼ Ãæµ¹");
        }

        transform.position += cam.transform.forward * force * Time.deltaTime;
    }

    private void Idle()
    {

    }

    public void IdleMassage()
    {
        Idle();
    }

    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }
}
