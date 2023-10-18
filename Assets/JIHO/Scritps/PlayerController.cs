using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public State<PlayerController> currentState;
    public State<PlayerController> previousState;

    [SerializeField] private float currentHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float jumpForce;
    [SerializeField] private float superJumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float groundTime;
    [SerializeField] private float groundMaxTime;
    [SerializeField] private bool isJump;

    [SerializeField] private Vector3 moveVec = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private PhysicMaterial pm;

    private bool isGround;
    private bool isMove;
    private bool isDash;
    private bool isSuperJump;
    
    private Vector3 heading = Vector3.zero;
    private Ray ray;

    public Rigidbody rigid;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;
    private PlayerDashState playerDashState;


    public Animator animator;

    public float RotateSpeed { get => rotateSpeed; }
    public float MoveSpeed { get => moveSpeed; }

    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public bool IsDash { get => isDash; set => isDash = value; }
    public bool IsGround { get => isGround; }

    public Vector3 MoveVec { get => moveVec; set => moveVec = value; }
    public Vector3 Heading { get => heading; set => heading = value; }

    public PlayerIdleState PlayerIdleState { get => playerIdleState; }
    public PlayerWalkState PlayerWalkState { get => playerWalkState; }
    public PlayerDashState PlayerDashState { get => playerDashState; }

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

        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();
        playerDashState = new PlayerDashState();

        currentState = playerIdleState;
        previousState = playerIdleState;
        pm.bounceCombine = PhysicMaterialCombine.Minimum;
    }

    private void Update()
    {
        RayCheck();
        
    }

    private void FixedUpdate()
    {
        currentState.StateUpdate(this);

        Walk();
        Debug.LogError(currentState);
    }

    private void Walk()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) return;
            moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVec.Normalize();

        heading = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        heading.Normalize();

        heading = heading - moveVec;

        float angle = Mathf.Atan2(heading.z, heading.x) * Mathf.Rad2Deg * -2;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * rotateSpeed);

        Vector3 dir = transform.forward * moveSpeed * Time.deltaTime;

        rigid.MovePosition(transform.position + dir);
    }

    public void RayCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, 0.05f, layer)) //Jump
        {
            isJump = true;
        }
    }

    public void Dash()
    {
        if (!CoolTimeManager.Instance.CoolCheck("Dash")) return;
        CoolTimeManager.Instance.GetCoolTime("Dash");

        pm.bounceCombine = PhysicMaterialCombine.Maximum;
        groundTime = groundMaxTime;
        isDash = true;

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        ray = new Ray(transform.position, dir);

        rigid.AddForce(ray.direction * dashSpeed, ForceMode.Impulse);

    }

    public void SuperJump()
    {
        if (!CoolTimeManager.Instance.CoolCheck("SuperJump")) return;
        CoolTimeManager.Instance.GetCoolTime("SuperJump");

        pm.bounceCombine = PhysicMaterialCombine.Maximum;
        groundTime = groundMaxTime;
        
        isSuperJump = true;

        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.down * superJumpForce, ForceMode.Impulse);
    }

    public void Jump()
    {
        if (!isJump) return;
        pm.bounceCombine = PhysicMaterialCombine.Minimum;
        groundTime = groundMaxTime;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJump = false;
    }

    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Object") && isDash)
        {
            isDash = false;
            CoolTimeManager.Instance.SetCoolTime("SuperJump", 0);
        }

        if ((collision.transform.CompareTag("Object") || collision.transform.CompareTag("Ground")) && isSuperJump)
        {
            isSuperJump = false;
            CoolTimeManager.Instance.SetCoolTime("Dash", 0);
        }


    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            if(pm.bounceCombine == PhysicMaterialCombine.Maximum)
            {   
                if(groundTime < 0)
                {
                    pm.bounceCombine = PhysicMaterialCombine.Minimum;
                }
                else
                {
                    groundTime -= Time.deltaTime;
                }
            }
        }
    }

}
