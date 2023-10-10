using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public State<PlayerController> currentState;

    [SerializeField] private float currentHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float rayDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float rotateSpeed;
    private bool isJump;
    private bool isMove;
    private bool isDash;

    [SerializeField] private Vector3 moveVec = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private PhysicMaterial pm;

    private float moveX;
    private float moveZ;

    private Vector3 dir = Vector3.zero;
    private Vector3 heading = Vector3.zero;

    private CapsuleCollider col;
    
    private Rigidbody rigid;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;
    private PlayerDashState playerDashState;

    public Animator animator;
    public TextMeshProUGUI textText;

    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public bool IsDash { get => isDash; set => isDash = value; }

    public float MoveX { get => moveX; set => moveX = value; }
    public float MoveZ { get => moveZ; set => moveZ = value; }
    
    public Vector3 MoveVec { get => moveVec; set => moveVec = value; }

    public PlayerIdleState PlayerIdleState { get => playerIdleState; }
    public PlayerWalkState PlayerWalkState { get => playerWalkState; }
    public PlayerDashState PlayerDashState { get => playerDashState; }


    private void Awake()
    {
        if(Instance == null)
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
        if(rigid == null) rigid = GetComponent<Rigidbody>();
        if (col == null) col = GetComponent<CapsuleCollider>();

        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();
        playerDashState = new PlayerDashState();
        
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

        moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVec.Normalize();

        heading = Camera.main.transform.forward;
        heading.y = 0;
        heading.Normalize();

        heading = heading - moveVec;

        float angle = Mathf.Atan2(heading.z, heading.x) * Mathf.Rad2Deg * -2;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * rotateSpeed);

        rigid.velocity = transform.forward * moveSpeed;

    }
   
    public void IdleEnter()
    {
        animator.SetBool("Walk", false);
        isMove = false;
    }

    public void Idle()
    {

    }

    public void DashEnter()
    {
        isDash = true;
        col.material = pm;
        rigid.drag = 1.2f;
        dir = Camera.main.transform.forward;
        transform.forward = new Vector3(transform.forward.x, transform.forward.y, dir.z);
        rigid.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
    }


    public void DashUpdate()
    {
        if (dashTime > 0) dashTime -= Time.fixedDeltaTime;
        else ChangeState(PlayerWalkState);

        //if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layer))
        //{
        //    if(Vector3.Distance(hit.point, transform.position) < 0.5f)
        //    {
        //        Vector3 incomingVector = transform.forward;
        //        incomingVector = incomingVector.normalized;
        //        Vector3 normalVector = hit.normal;
        //        Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
        //        reflectVector = reflectVector.normalized;
        //        dir = reflectVector;
        //        transform.forward = new Vector3(transform.forward.x, dir.y, transform.forward.z);
        //        dir = transform.forward;
        //    }
        //}

            
    }

    public void DashExit()
    {
        isDash = false;
        dashTime = 2f;
        col.material = null;
        rigid.drag = 10;
    }

    public void WallCheck()
    {
        
    }


    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }
}
