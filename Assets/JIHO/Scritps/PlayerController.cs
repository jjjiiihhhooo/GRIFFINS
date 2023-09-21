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
    [SerializeField] private float rayDistance;

    [SerializeField] private float bulletCoolTime;
    [SerializeField] private float bulletCurTime;

    [SerializeField] private float mouseSpeed;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

    [SerializeField] private float forceMax;
    [SerializeField] private float force;
    [SerializeField] private float scopeForce;
    [SerializeField] private float tempForce;

    [SerializeField] private int pinballCount;
    [SerializeField] private int pinballMaxCount;

    [SerializeField] private bool isMouse;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isScope;

    public bool isPinball;

    [SerializeField] private Vector2 moveVec = Vector2.zero;
    [SerializeField] private Vector3 camPos = Vector3.zero;
    [SerializeField] private Vector3 camScopePos = Vector3.zero;
    [SerializeField] private Vector3 PinBallLandingPos = Vector3.zero;

    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform firePos;
    [SerializeField] private Transform playerCharacter;
    [SerializeField] private Vector3[] routeVectors;
    [SerializeField] private LineRenderer line;

    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject scope;

    public Rigidbody rigid;
    public CapsuleCollider col;
    public MainCamera cam;

    public Animator animator;
    public Animator attackerAnimator;

    private PlayerIdleState playerIdleState;
    private PlayerWalkState playerWalkState;
    private PinBallReadyState pinBallReadyState;
    private PlayerPinballState playerPinballState;

    public bool IsMouse { get => isMouse; }
    public bool IsPinball { get => isPinball; }
    public bool IsScope { get => isScope; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }

    public PlayerIdleState PlayerIdleState { get => playerIdleState; }
    public PlayerWalkState PlayerWalkState { get => playerWalkState; }
    public PinBallReadyState PinBallReadyState { get => pinBallReadyState; }
    public PlayerPinballState PlayerPinballState { get => playerPinballState; }

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

    private void Init()
    {
        playerIdleState = new PlayerIdleState();
        playerWalkState = new PlayerWalkState();
        pinBallReadyState = new PinBallReadyState();
        playerPinballState = new PlayerPinballState();

        MouseCursorSet(false);

        currentState = playerIdleState;
    }


    private void FixedUpdate()
    {
        currentState.StateUpdate(this);
    }

    private void Update()
    {
        Debug.LogError(currentState.ToString());
        if(Input.GetMouseButtonDown(1))
        {
            ScopeMode();
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (!isScope) return;
            Shoot();
        }

        if (Input.GetMouseButton(0))
        {
            if (!isScope) return;
            Shoot();
        }  
        if (bulletCurTime > 0) bulletCurTime -= Time.deltaTime;
    }

    public void WalkEnter()
    {
        animator.SetBool("Walk", true);
    }

    public void Walk()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");

        isMove = moveVec.magnitude != 0;

        Vector3 lookForward = new Vector3(cam.cameraArm.forward.x, 0f, cam.cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cam.cameraArm.right.x, 0f, cam.cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveVec.y + lookRight * moveVec.x;

        playerCharacter.forward = lookForward;

        if (isMove) transform.position += moveDir * Time.deltaTime * CurrentSpeed;
    }


    public void IdleEnter()
    {
        animator.SetBool("Walk", false);
    }

    public void Idle()
    {

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
        if (!isPinball) return;
        Debug.Log("ÇÉº¼ ½ÃÀÛ");
        pinBallMoveEffect.SetActive(true);
        attackerAnimator.gameObject.SetActive(false);
        
        
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PinBallMove")) animator.SetTrigger("Dash");

        if (pinballCount >= pinballMaxCount)
        {
            
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, routeVectors[pinballCount], Time.deltaTime * force);

        if(Physics.Raycast(playerCharacter.transform.position, routeVectors[pinballCount] - transform.position, out hit, Mathf.Infinity, enemyLayer))
        {
            if (Vector3.Distance(transform.position, hit.point) < 1f)
            {
                GameObject temp = Instantiate(hit.transform.GetComponent<Enemy>().damageEffect, transform.position, Quaternion.identity);
                //hit.transform.GetComponent<Enemy>().anim.SetTrigger("Hit");
            }
        }

        if (Physics.Raycast(playerCharacter.transform.position, routeVectors[pinballCount] - transform.position, out hit, Mathf.Infinity, layer))
        {
            if (Vector3.Distance(transform.position, hit.point) < 1f) if (hit.transform.gameObject.CompareTag("item")) hit.transform.GetComponent<PlatformObj>().anim.SetTrigger("Hit");

        }

        playerCharacter.forward = routeVectors[pinballCount] - transform.position;



        if (Vector3.Distance(transform.position, routeVectors[pinballCount]) < 0.1f) 
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PinBallLanding")) 
                animator.SetTrigger("Landing");

            
            pinballCount++;
            GameObject temp = Instantiate(attackEffect, transform.position, Quaternion.identity);

            

                if (force < forceMax && !isScope) force += 2;

            temp.SetActive(true);
        }

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
        force = tempForce;
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
            force = scopeForce;
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

            obj.transform.position = cam.transform.position + cam.transform.forward;
            bullet.direction = cam.rayDir.normalized;
            obj.SetActive(true);

        }
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
