using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

using Sirenix.OdinInspector;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerResizableCapsuleCollider))]
public class Player : SerializedMonoBehaviour
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
    public Swinging swinging;
    public Canvas playerCanvas;
    public SpawnPoint spawn;
    public UnityEngine.UI.Image staminaFill;
    public PlayerMovementStateMachine movementStateMachine;

    public PlayerCharacter currentCharacter;
    public PlayerCharacter[] characters;

    public Animator[] animators;

    public Vector3 dir;
    public Ray ray;
    public Ray testRay;
    public Ray testRay1;
    public Ray testRay2;
    
    public bool isGround; //땅에 있는 상태인지

    public bool isGrapple; //그래플링 상태인지

    public bool isAttack; //공격 상태인지

    public bool isPsyche; //염력 상태인지

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
            currentCharacter = characters[0];
            currentCharacter.model.SetActive(true);
            Animator = currentCharacter.animator;


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
        if (skillData.touch) return;
        if (swinging.swinging) return;
        if (currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") || currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw"))
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
        if (skillData.touch) return;
        if (swinging.swinging) return;
        movementStateMachine.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (skillData.touch)
        {
            skillData.touch = false;
            currentCharacter.StopGrapple(this);
            //skillData.StopGrapple();
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
        if (skillData.touch)
        {
            skillData.touch = false;
            currentCharacter.StopGrapple(this);
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

    public void ChangeCharacter(int index)
    {
        if (index == currentCharacter.index) return;

        characters[index].model.SetActive(true);
        foreach (AnimatorControllerParameter paramA in currentCharacter.animator.parameters)
        {
            switch (paramA.type)
            {
                case AnimatorControllerParameterType.Bool:
                    characters[index].animator.SetBool(paramA.name, currentCharacter.animator.GetBool(paramA.name));
                    break;
                case AnimatorControllerParameterType.Float:
                    characters[index].animator.SetFloat(paramA.name, currentCharacter.animator.GetFloat(paramA.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    characters[index].animator.SetInteger(paramA.name, currentCharacter.animator.GetInteger(paramA.name));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    if (currentCharacter.animator.GetBool(paramA.name))
                    {
                        characters[index].animator.SetTrigger(paramA.name);
                    }
                    break;
            }
        }
        currentCharacter.model.SetActive(false);
        currentCharacter = characters[index];
        Animator = currentCharacter.animator;

    }

    public void CoroutineEvent(IEnumerator enumerator)
    {
        StopCoroutine(enumerator);
        StartCoroutine(enumerator);
    }

    public void DestroyEvent(UnityEngine.Object obj, float delay = 0f)
    {
        Destroy(obj, delay);
    }

    public GameObject InstantiateEvent(GameObject obj, Vector3 transform, Quaternion quaternion)
    {
       return Instantiate(obj, transform, quaternion);
    }

}



