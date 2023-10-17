using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public State<PlayerController> currentState;
    public State<PlayerController> previousState;

    [SerializeField] private float currentHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float dashGravity;
    [SerializeField] private float idleGravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float downSpeed;
    [SerializeField] private float superJumpSpeed;
    [SerializeField] private float jumpTime;
    [SerializeField] private float test;
    [SerializeField] private float superJumpTime;
    [SerializeField] private float superTest;


    [SerializeField] private bool isJump;
    private bool isGround;
    private bool isMove;
    private bool isDash;
    private bool superJumpGround;

    [SerializeField] private Vector3 moveVec = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private PhysicMaterial pm;

    private Vector3 dir = Vector3.zero;
    private Vector3 heading = Vector3.zero;

    private CapsuleCollider col;

    private Rigidbody rigid;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;
    private PlayerDashState playerDashState;
    private PlayerSuperJumpState playerSuperJumpState;


    public Animator animator;

    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public bool IsDash { get => isDash; set => isDash = value; }
    public bool IsGround { get => isGround; }

    public Vector3 MoveVec { get => moveVec; set => moveVec = value; }

    public PlayerIdleState PlayerIdleState { get => playerIdleState; }
    public PlayerWalkState PlayerWalkState { get => playerWalkState; }
    public PlayerDashState PlayerDashState { get => playerDashState; }
    public PlayerSuperJumpState PlayerAirState { get => playerSuperJumpState; }

    public float dashclear = 5f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
        if (col == null) col = GetComponent<CapsuleCollider>();

        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();
        playerDashState = new PlayerDashState();
        playerSuperJumpState = new PlayerSuperJumpState();

        currentState = playerIdleState;
        previousState = playerIdleState;
    }

    private void Update()
    {
        GroundCheck();
    }

    private void FixedUpdate()
    {
        currentState.StateUpdate(this);
        Debug.LogError(currentState);
    }


    public void WalkEnter()
    {
        animator.SetBool("Walk", true);
        isMove = true;
    }

    public void Walk()
    {

        moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVec.Normalize();

        heading = Camera.main.transform.forward;
        heading.y = 0;
        heading.Normalize();

        heading = heading - moveVec;

        float angle = Mathf.Atan2(heading.z, heading.x) * Mathf.Rad2Deg * -2;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * rotateSpeed);

        rigid.velocity = new Vector3(transform.forward.x * moveSpeed, rigid.velocity.y, transform.forward.z * moveSpeed);

    }

    public void IdleEnter()
    {
        animator.SetBool("Walk", false);
        isMove = false;
    }

    public void Idle()
    {

    }

    public void GroundCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, 0.05f, layer)) isGround = true;
        else isGround = false;
        Debug.DrawRay(transform.position, -transform.up, Color.red, 0.08f);
    }

    public void DashEnter()
    {
        isDash = true;
        col.material = pm;
        rigid.drag = 1.2f;
        Physics.gravity = new Vector3(0, dashGravity, 0);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layer)) dir = hit.point - transform.position;
        else dir = Camera.main.transform.forward;
        
        dir.Normalize();
        rigid.velocity = Vector3.zero;
        rigid.AddForce(dir * dashSpeed, ForceMode.Impulse);
    }

    public void DashUpdate()
    {
        if (dashTime > 0) dashTime -= Time.fixedDeltaTime;
        else ChangeState(playerIdleState);
    }

    public void DashExit()
    {
        dashTime = dashclear;
        isDash = false;
        rigid.drag = 10;
        col.material = null;
        Physics.gravity = new Vector3(0, idleGravity, 0);
    }

    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }
}
