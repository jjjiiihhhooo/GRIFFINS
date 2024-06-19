using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerResizableCapsuleCollider))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Collisions")]
    public AttackCol normalAttackCol;
    public AttackCol normalAttackCol_2;
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    [field: Header("Camera")]
    [field: SerializeField] public PlayerCameraUtility CameraRecenteringUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public TextMeshProUGUI text;



    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }

    public PlayerInput Input { get; private set; }
    public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public TargetSet targetSet;
    public SkillData skillData;
    public Swinging swinging;
    public SpawnPoint spawn;
    public ObjectTrigger curTrigger;
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

    [Header("WhiteCharacter")]
    public WhiteCharacter whiteCharacter;

    [Header("GreenCharacter")]
    public GreenCharacter greenCharacter;

    [Header("RedCharacter")]
    public RedCharacter redCharacter;

    public Vector3 orginPos;

    public float maxHp;
    public float curHp;
    public int deathCount = 0;


    //1: white
    //2: green
    //3: blue
    //4: red


    public Vector3 dir;
    public Ray ray;
    public Ray camRay;

    public bool isGround; //땅에 있는 상태인지

    public bool isAttack; //공격 상태인지
    public bool isNormalAttack;


    public bool isItemSave;

    public bool backHpHit;

    public bool freeze;
    public bool playerHit;

    public bool isSuperAttack;
    public bool isSuperAttacking;

    public bool isDead;

    public float maxHitCool;
    public float curHitCool;
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
            characters = new PlayerCharacter[3];
            characters[0] = whiteCharacter;
            characters[1] = greenCharacter;
            characters[2] = redCharacter;

            for (int i = 0; i < characters.Length; i++)
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
        if (GameManager.Instance.isCutScene) return;
        if (isDead) return;

        if (playerHit)
        {
            curHitCool -= Time.deltaTime;
            if (curHitCool < 0)
            {
                playerHit = false;
            }
        }

        currentCharacter.Update();
        //UIUpdate();
        dir = MainCameraTransform.forward;
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), dir);
        camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        //if (skillData.touch) return;
        //if (swinging.swinging) return;
        if (currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") || currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw"))
        {
            transform.rotation = CameraRecenteringUtility.VirtualCamera.transform.rotation;
            transform.rotation = new Quaternion(0f, CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, transform.rotation.w);
        }

        movementStateMachine.HandleInput();

        movementStateMachine.Update();
    }


    private void FixedUpdate()
    {
        //if (skillData.touch) return;
        //if (swinging.swinging) return;
        movementStateMachine.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //if (skillData.touch)
        //{
        //    skillData.touch = false;

        //    currentCharacter.StopGrapple();
        //    //skillData.StopGrapple();
        //}

        //if (swinging.swinging)
        //{
        //    swinging.swinging = false;
        //    swinging.StopSwing();
        //}

        if (collider.tag == "EnemyAttackCol")
        {
            //if (movementStateMachine.CurStateName() == "PlayerDashingState") return;
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
        isDead = true;
        currentCharacter.animator.Play("Die", 3, 0f);
        GameManager.Instance.uiManager.fade.GetComponent<NoticeContainer>().StartNotice();


    }

    public void DeadNotice()
    {
        isDead = false;
        isAttack = false;
        isNormalAttack = false;
        isSuperAttacking = false;
        isSuperAttack = false;
        playerHit = false;
        deathCount++;
        //Destroy(GameManager.Instance.enemyManager.curWaveObject);
        //GameManager.Instance.questManager.QuestDestoryEvent();
        //if (curTrigger != null) curTrigger.GetComponent<Collider>().enabled = true;
        curHp = maxHp;
        GameManager.Instance.uiManager.FadeInOut();
        currentCharacter.animator.Play("Up", 3, 0f);
        PlayerSpawn();
    }

    public void PlayerSpawn()
    {
        GameManager.Instance.uiManager.deathCount_text.text = "Death : " + deathCount;
        //spawn.Spawn();
    }

    public void GetDamage(float damage)
    {
        if (playerHit) return;
        if (GameManager.Instance.isCutScene) return;
        if (movementStateMachine.CurStateName() == "PlayerDashingState") return;
        playerHit = true;
        curHitCool = maxHitCool;
        curHp -= damage;
        if (curHp > 30)
            GameManager.Instance.uiManager.PlayerHitUI(true);
        else
            GameManager.Instance.uiManager.PlayerHitUI(false);
        backHpHit = false;
        if (curHp <= 0) PlayerDead();
        Invoke("BackHpMessage", 0.6f);
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
        if (!GameManager.Instance.coolTimeManager.CoolCheck("CharacterChange")) return;

        if (index == 1 && GameManager.Instance.questManager.isInput) GameManager.Instance.questManager.InputQuestCheck(KeyCode.Alpha1);
        else GameManager.Instance.questManager.InputQuestCheck(KeyCode.Alpha2);

        GameManager.Instance.coolTimeManager.GetCoolTime("CharacterChange");
        isSuperAttack = true;
        GameManager.Instance.uiManager.CharacterCharacter(index);
        currentCharacter.CharacterChange();
        //GameManager.Instance.uiManager.ChangeCharacterUI(index);

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

    public void CoroutineExit(IEnumerator enumerator)
    {
        StopCoroutine(enumerator);
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
        if (_bool)
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



