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

    [SerializeField] private float bulletCoolTime;
    [SerializeField] private float bulletCurTime;

    [SerializeField] private float mouseSpeed;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

    [SerializeField] private float force = 50.0f;
    [SerializeField] private float tempForce;

    [SerializeField] private int pinballCount;
    [SerializeField] private int pinballMaxCount;

    [SerializeField] private bool isAttack;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isReroad;
    [SerializeField] private bool isMouse;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isPinball;
    [SerializeField] private bool isScope;

    [SerializeField] private Vector2 moveVec = Vector2.zero;
    [SerializeField] private Vector3 vector = Vector3.zero;
    [SerializeField] private Vector3 camPos = Vector3.zero;
    [SerializeField] private Vector3 camScopePos = Vector3.zero;
    [SerializeField] private Vector3 playerRotate = Vector3.zero;
    [SerializeField] private Vector3 pinballRoutePos = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform playerCharacter;
    [SerializeField] private Vector3[] routeVectors;
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject scope;

    [SerializeField] private MainCamera cam;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;

    public bool IsMouse { get => isMouse; }
    public bool IsPinball { get => isPinball; }
    public bool IsScope { get => isScope; }
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

        MouseCursorSet(false);

        currentState = playerIdleState;
    }


    private void FixedUpdate()
    {
        Move();
        PinBall();
        PinBallRoute();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPinball = true;
            rigid.useGravity = false;
            col.enabled = false;
            playerCharacter.forward = cam.transform.forward;
        }

        if(Input.GetMouseButtonDown(1))
        {
            ScopeMode();
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (!isScope) return;
            Shoot();
        }

        if (bulletCurTime > 0) bulletCurTime -= Time.deltaTime;
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

    private void PinBallRoute()
    {
        if (isPinball) return;

        pinballRoutePos = playerCharacter.position;
        Debug.Log("1" + pinballRoutePos);
        Vector3 routeVec = cam.mousePos - pinballRoutePos;

        for(int i = 0; i < pinballMaxCount; i++)
        {
            if (Physics.Raycast(pinballRoutePos, routeVec, out hit, Mathf.Infinity, layer))
            {
                line.SetPosition(i, pinballRoutePos);
                Vector3 incomingVector = routeVec;
                incomingVector = incomingVector.normalized;
                Vector3 normalVector = hit.normal;
                Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
                reflectVector = reflectVector.normalized;
                routeVec = reflectVector;
                pinballRoutePos = hit.point;
                routeVectors[i] = hit.point;
            }
        }
    }

    private void PinBall()
    {
        if (!isPinball) return;
        Debug.Log("ÇÉº¼ ½ÃÀÛ");

        if(pinballCount >= pinballMaxCount)
        {
            isPinball = false;
            col.enabled = true;
            pinballCount = 0;
            rigid.useGravity = true;
            force = tempForce;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, routeVectors[pinballCount], Time.deltaTime * force);

        playerCharacter.forward = routeVectors[pinballCount] - transform.position;
        //if (!isScope)
        //{
        //    cam.cameraArm.forward = routeVectors[pinballCount] - transform.position;
        //}
        //else
        //{
        //    cam.transform.position = playerCharacter.transform.position + playerCharacter.transform.forward * 0.2f;
        //}

        if (Vector3.Distance(transform.position, routeVectors[pinballCount]) < 0.1f) pinballCount++;

    }

    private void ScopeMode()
    {
        isScope = !isScope;

        if (!isScope)
        {
            cam.transform.localPosition = camPos;
            force = tempForce;
        }
        else
        {
            cam.transform.localPosition = camScopePos;
            tempForce = force;
            force = 0.3f;
        }

        scope.SetActive(isScope);
    }

    private void Shoot()
    {
        if(bulletCurTime <= 0)
        {
            bulletCurTime = bulletCoolTime;
            GameObject obj = Managers.Instance.BulletSpawner.PopQueue();
            Bullet bullet = obj.GetComponent<Bullet>();

            obj.transform.position = cam.transform.position;
            bullet.direction = cam.transform.forward;
            obj.SetActive(true);

        }
    }

    private void Idle()
    {

    }

    public void IdleMassage()
    {
        Idle();
    }

    public void MouseCursorSet(bool _bool)
    {
        isMouse = _bool;
        Cursor.visible = _bool;
        if (_bool) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    public void ChangeState(State<PlayerController> state)
    {
        state.StateChange(this);
    }
}
