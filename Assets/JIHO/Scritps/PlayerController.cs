using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private bool isAttack;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isReroad;
    [SerializeField] private bool isMouse;

    [SerializeField] private Vector3 vector = Vector3.zero;
    [SerializeField] private Vector3 camVector = Vector3.zero;
    [SerializeField] private Vector3 playerRotate = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform firePos;

    [SerializeField] private Camera cam;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;

    public bool IsMouse { get => isMouse; }
    public PlayerIdleState PlayerIdleState { get => playerIdleState ;}
    public PlayerWalkState PlayerWalkState { get => playerWalkState ;}



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
        Rotate();
    }

    private void Move()
    {
        
    }

    private void Rotate()
    {

    }

    public void ChangeState(State<PlayerController> state)
    {

    }
}
