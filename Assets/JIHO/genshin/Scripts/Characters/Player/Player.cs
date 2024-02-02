using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;



[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerResizableCapsuleCollider))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Collisions")]
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    [field: Header("Camera")]
    [field: SerializeField] public PlayerCameraUtility CameraRecenteringUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public GameObject jumpEffect;
    public GameObject dashEffect;
    public GameObject landEffect;


    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }

    public PlayerInput Input { get; private set; }
    public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public TargetSet targetSet;
    public SkillData skillData;
    public SkillFunction skillFunction;
    public Swinging swinging;
    public Canvas playerCanvas;
    public SpawnPoint spawn;
    public UnityEngine.UI.Image staminaFill;
    public PlayerMovementStateMachine movementStateMachine;

    public Vector3 dir;
    public Ray ray;
    public Ray testRay;
    public Ray testRay1;
    public Ray testRay2;
    
    public bool isGround;

    public bool isGrapple;

    public float testHp;

    private void Awake()
    {
        if (Instance == null)
        {
            if (playerCanvas.worldCamera == null) playerCanvas.worldCamera = Camera.main;
            Instance = this;
            CameraRecenteringUtility.Initialize();
            AnimationData.Initialize();
            swinging = GetComponent<Swinging>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            Input = GetComponent<PlayerInput>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            MainCameraTransform = Camera.main.transform;

            movementStateMachine = new PlayerMovementStateMachine(this);

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void Update()
    {
        dir = MainCameraTransform.forward;
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), dir);
        if (skillFunction.touch) return;
        if (swinging.swinging) return;
        if (Animator.GetCurrentAnimatorStateInfo(1).IsName("White_Idle") || Animator.GetCurrentAnimatorStateInfo(1).IsName("White_Throw"))
        {
            Debug.Log("anim");
            transform.rotation = CameraRecenteringUtility.VirtualCamera.transform.rotation;
            transform.rotation = new Quaternion(0f, CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, transform.rotation.w);
        }

        movementStateMachine.HandleInput();

        movementStateMachine.Update();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(testRay.origin, testRay.direction, Color.red);    
        Debug.DrawRay(testRay1.origin, testRay.direction, Color.red);    
        Debug.DrawRay(testRay2.origin, testRay.direction, Color.red);    
    }

    private void FixedUpdate()
    {
        if (skillFunction.touch) return;
        if (swinging.swinging) return;
        movementStateMachine.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (skillFunction.touch)
        {
            skillFunction.touch = false;
            skillData.StopGrapple();
        }

        if(swinging.swinging)
        {
            swinging.swinging = false;
            swinging.StopSwing();
        }

        if(collider.tag == "Laser")
        {
            PlayerDead();
        }

        movementStateMachine.OnTriggerEnter(collider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (skillFunction.touch)
        {
            skillFunction.touch = false;
            skillData.StopGrapple();
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        if (!isGround) isGround = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (isGround) isGround = false;
        movementStateMachine.OnTriggerExit(collider);
    }

    public void PlayerDead()
    {
        spawn.Spawn();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
}



