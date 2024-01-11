using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace genshin
{
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

        public PlayerMovementStateMachine movementStateMachine;



        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                CameraRecenteringUtility.Initialize();
                AnimationData.Initialize();

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
            if(skillData.isHand)
            {
                transform.rotation = FindObjectOfType<CameraZoom>().transform.rotation;
                transform.rotation = new Quaternion(0f, FindObjectOfType<CameraZoom>().transform.rotation.y, 0f, transform.rotation.w);
            }

            movementStateMachine.HandleInput();

            movementStateMachine.Update();
        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate();
        }

        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
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
}


