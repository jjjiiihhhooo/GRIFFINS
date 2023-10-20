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
    [SerializeField] private bool isGround;

    [SerializeField] private Vector3 moveVec = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private PhysicMaterial pm;

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
        isJump = true;
    }

    private void Update()
    {
        RayCheck();
    }

    private void FixedUpdate()
    {
        currentState.StateUpdate(this);

        //Debug.LogError(currentState);
    }

    public void RayCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, 0.05f, layer)) //Jump
        {
            if(!isJump)
            {
                isJump = true;
                
                animator.SetBool("isJump", false);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("DashAir") && isDash)
            {
                Debug.LogError("dd");
                isDash = false;
                animator.SetBool("isDashAir", false);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ObjectHitAir"))
            {
                animator.SetBool("isObjectAir", false);
            }

            isGround = true;
        }
        else
        {
            isGround = false;
        }

        
    }

    public void Dash()
    {
        if (!CoolTimeManager.Instance.CoolCheck("Dash")) return;
        CoolTimeManager.Instance.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        pm.bounceCombine = PhysicMaterialCombine.Maximum;
        groundTime = groundMaxTime;
        isDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        ray = new Ray(transform.position, dir);

        rigid.AddForce(ray.direction * dashSpeed, ForceMode.Impulse);
        transform.forward = ray.direction;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
    }

    public void SuperJump()
    {
        if (!CoolTimeManager.Instance.CoolCheck("SuperJump")) return;
        CoolTimeManager.Instance.GetCoolTime("SuperJump");

        isDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GroundDown") && !isGround)
        {
            isSuperJump = true;
            animator.SetBool("GroundReady", false);
            animator.SetTrigger("GroundDown");
            Debug.LogError("qq");
        }
        else
        {
            isSuperJump = false;
            animator.SetTrigger("GroundReadyAction");
        }

        pm.bounceCombine = PhysicMaterialCombine.Maximum;
        groundTime = groundMaxTime;

        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.down * superJumpForce, ForceMode.Impulse);
    }

    public void Jump()
    {
        if (!isJump || animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) return;

        if (isDash) isDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) animator.SetTrigger("JumpReady");
        animator.SetBool("isJump", true);

        pm.bounceCombine = PhysicMaterialCombine.Minimum;
        groundTime = groundMaxTime;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Invoke("JumpAir", 0.1f);
    }

    private void JumpAir()
    {
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

            animator.SetBool("isObjectAir", true);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ObjectHit")) animator.SetTrigger("ObjectHit");
            CoolTimeManager.Instance.SetCoolTime("SuperJump", 0);
            
        }

        if ((collision.transform.CompareTag("Object") || collision.transform.CompareTag("Ground")) && isSuperJump)
        {
            isSuperJump = false;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GroundReady")) animator.SetBool("GroundReady",true);
            CoolTimeManager.Instance.SetCoolTime("Dash", 0);
        }

        if(collision.transform.CompareTag("Object") || collision.transform.CompareTag("Ground"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundJump") || animator.GetCurrentAnimatorStateInfo(0).IsName("GroundJumpAir"))
            {
                animator.SetTrigger("GroundExit");
            }

            
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
