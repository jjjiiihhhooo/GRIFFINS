using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace genshin
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerResizableCapsuleCollider))]
    public class Player : MonoBehaviour
    {
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
        public PlayerInteraction PlayerInteraction;

        public PlayerMovementStateMachine movementStateMachine;


        public GameObject tornadoSkillObject;
        public AttackCol attackCol;
        public AttackCol dashCol;
        public bool isInteraction;
        public bool isSkill;
        public float damage;
        public float groundTime;
        public float groundMaxTime;
        public PhysicMaterial pm;

        public Ray ray;
        public Vector3 dir;

        private void Awake()
        {
            CameraRecenteringUtility.Initialize();
            AnimationData.Initialize();
            PlayerInteraction = GetComponent<PlayerInteraction>();

            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            Input = GetComponent<PlayerInput>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            MainCameraTransform = Camera.main.transform;

            movementStateMachine = new PlayerMovementStateMachine(this);

            isInteraction = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(ray);
        }

        private void Start()
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
        }

        private void Update()
        {
            dir = MainCameraTransform.forward;
            ray = new Ray(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), dir);

            movementStateMachine.HandleInput();

            movementStateMachine.Update();
            

            if(UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate();

            PlayerInteraction.PhysicsUpdate();
        }

        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            movementStateMachine.OnTriggerStay(collider);
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

        public void StartCor(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void StopCor(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }

        public void AttackColActive(float time = 0.2f)
        {
            attackCol.gameObject.SetActive(false);
            attackCol.damage = damage;
            attackCol.time = time;
            attackCol.gameObject.SetActive(true);
        }

        public void DashColActive(float time = 0f)
        {
            dashCol.gameObject.SetActive(false);
            dashCol.damage = damage;
            dashCol.time = time;
            dashCol.gameObject.SetActive(true);
        }

        
    }
}


