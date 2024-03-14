using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using System.Transactions;
using UnityEngine.Rendering.Universal;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerResizableCapsuleCollider))]
public class Player : SerializedMonoBehaviour
{
    public static Player Instance;

    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Collisions")]
    public AttackCol attackCol;
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    [field: Header("Camera")]
    [field: SerializeField] public PlayerCameraUtility CameraRecenteringUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }


    


    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }

    public PlayerInput Input { get; private set; }
    public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public TargetSet targetSet;
    public SkillData skillData;
    public Swinging swinging;
    public SpawnPoint spawn;
    public PlayerMovementStateMachine movementStateMachine;

    
    [Header("GameObject")]
    public GameObject jumpEffect;
    public GameObject dashEffect;
    public GameObject landEffect;
    public GameObject saveItem;

    [Header("UI")]
    public Canvas playerCanvas;
    public UnityEngine.UI.Image staminaFill;
    public UnityEngine.UI.Image interactionImage;
    public TextMeshProUGUI interactionText;

    [Header("Character")]
    public PlayerCharacter currentCharacter;
    public PlayerCharacter[] characters;

    public float maxHp;
    public float curHp;


    //1: white
    //2: green
    //3: blue
    //4: red


    public Vector3 dir;
    public Ray ray;
    public Ray camRay;
    
    public bool isGround; //땅에 있는 상태인지

    public bool isAttack; //공격 상태인지

    public bool isItemSave;

    public bool backHpHit;

    public float testHp;


    private void Awake()
    {
        if (Instance == null)
        {
            movementStateMachine = new PlayerMovementStateMachine(this);
            if (playerCanvas.worldCamera == null) playerCanvas.worldCamera = Camera.main;
            Instance = this;
            CameraRecenteringUtility.Initialize();
            AnimationData.Initialize();
            swinging = GetComponent<Swinging>();
            Rigidbody = GetComponent<Rigidbody>();
            curHp = maxHp;
            for(int i = 0; i < characters.Length; i++)
            {
                characters[i].Init(this);
            }

            currentCharacter = characters[0];
            currentCharacter.model.SetActive(true);

            Animator = currentCharacter.animator;



            Input = GetComponent<PlayerInput>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            MainCameraTransform = Camera.main.transform;

            

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
        if (GameManager.Instance.dialogueManager.IsChat) return;

        currentCharacter.Update();
        UIUpdate();
        dir = MainCameraTransform.forward;
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), dir);
        camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (skillData.touch) return;
        if (swinging.swinging) return;
        if (currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") || currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw"))
        {
            transform.rotation = CameraRecenteringUtility.VirtualCamera.transform.rotation;
            transform.rotation = new Quaternion(0f, CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, transform.rotation.w);
        }

        movementStateMachine.HandleInput();

        movementStateMachine.Update();
    }

    private void UIUpdate()
    {
        GameManager.Instance.uiManager.playerHp.value = Mathf.Lerp(GameManager.Instance.uiManager.playerHp.value, curHp / maxHp, Time.deltaTime * 5f);

        if (backHpHit)
        {
            GameManager.Instance.uiManager.playerBackHp.value = Mathf.Lerp(GameManager.Instance.uiManager.playerBackHp.value, GameManager.Instance.uiManager.playerHp.value, Time.deltaTime * 6f);
            if (GameManager.Instance.uiManager.playerHp.value >= GameManager.Instance.uiManager.playerBackHp.value - 0.001f)
            {
                backHpHit = false;
                GameManager.Instance.uiManager.bossBackHp.value = GameManager.Instance.uiManager.playerHp.value;
            }
        }
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
            currentCharacter.StopGrapple();
            //skillData.StopGrapple();
        }

        if (swinging.swinging)
        {
            swinging.swinging = false;
            swinging.StopSwing();
        }


        if(collider.tag == "EnemyAttackCol")
        {
            if (movementStateMachine.CurStateName() == "PlayerDashingState") return;
            GetDamage(collider.GetComponent<AttackCol>().damage);
        }

        if (collider.tag == "Spawn")
        {
            collider.GetComponent<SpawnPoint>().SetSpawn();
        }

        movementStateMachine.OnTriggerEnter(collider);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (skillData.touch)
    //    {
    //        skillData.touch = false;
    //        currentCharacter.StopGrapple();
    //    }

    //}

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
        
    }

    public void PlayerSpawn()
    {
        spawn.Spawn();
    }

    public void GetDamage(float damage)
    {
        curHp -= damage;
        backHpHit = false;
        if (curHp <= 0) PlayerDead();
        Invoke("BackHpMessage", 0.3f);
    }

    private void BackHpMessage()
    {
        backHpHit = true;
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

        currentCharacter.CharacterChange();

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

    public void EndCombo()
    {
        currentCharacter.comboCounter = 0;
        currentCharacter.lastComboEnd = Time.time;
    }

    public void SetActiveInteraction(bool _bool, string text = "")
    {
        if(_bool)
        {
            interactionText.text = text;
            interactionImage.gameObject.SetActive(true);
        }
        else
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    public GameObject InstantiateEvent(GameObject obj, Vector3 transform, Quaternion quaternion)
    {
       return Instantiate(obj, transform, quaternion);
    }

}



