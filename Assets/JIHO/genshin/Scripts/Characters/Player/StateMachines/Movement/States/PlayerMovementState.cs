
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;

        protected readonly PlayerGroundedData groundedData;
        protected readonly PlayerAirborneData airborneData;

        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;

            groundedData = stateMachine.Player.Data.GroundedData;
            airborneData = stateMachine.Player.Data.AirborneData;

            InitializeData();
        }

        public virtual void Enter()
        {
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag("QuestPos"))
            {
                QuestManager.instance.QuestPositionCheck(collider.name);
            }

            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);

                return;
            }
        }

        public virtual void OnTriggerStay(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                if(!stateMachine.Player.isGround) stateMachine.Player.isGround = true;

                if (stateMachine.Player.pm.bounceCombine == PhysicMaterialCombine.Maximum)
                {
                    if (stateMachine.Player.groundTime < 0)
                    {
                        stateMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Minimum;
                        stateMachine.ChangeState(stateMachine.IdlingState);
                    }
                    else
                    {
                        stateMachine.Player.groundTime -= Time.deltaTime;
                    }
                }
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);

                return;
            }
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        private void InitializeData()
        {
            SetBaseCameraRecenteringData();

            SetBaseRotationData();
        }

        protected void EffectActive(GameObject gameObject, bool _bool)
        {
            gameObject.SetActive(_bool);
        }

        protected void SetBaseCameraRecenteringData()
        {
            stateMachine.ReusableData.SidewaysCameraRecenteringData = groundedData.SidewaysCameraRecenteringData;
            stateMachine.ReusableData.BackwardsCameraRecenteringData = groundedData.BackwardsCameraRecenteringData;
        }

        protected void SetBaseRotationData()
        {
            stateMachine.ReusableData.RotationData = groundedData.BaseRotationData;

            stateMachine.ReusableData.TimeToReachTargetRotation = stateMachine.ReusableData.RotationData.TargetRotationReachTime;
        }

        protected void StartAnimation(int animationHash, int check = 0)
        {
            if(check == 1)
            {
                stateMachine.Player.Animator.SetTrigger(animationHash);
                return;
            }

            stateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash, int check = 0)
        {
            if(check == 1)
            {
                return;
            }
            stateMachine.Player.Animator.SetBool(animationHash, false);
        }

        protected virtual void AddInputActionsCallbacks()
        {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            stateMachine.Player.Input.PlayerActions.Look.started += OnMouseMovementStarted;

            stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            stateMachine.Player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;

            stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        }

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }

        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(context.ReadValue<Vector2>());
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            DisableCameraRecentering();
        }

        private void ReadMovementInput()
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        
        private void Move()
        {
            //if (stateMachine.GetCurrentStateType() == typeof(PlayerDashingState) || stateMachine.GetCurrentStateType() == typeof(PlayerAirDashingState))
            //{
            //    return;
            //}
            
            

            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                Debug.Log("MoveError");
                return;
            }

            if (stateMachine.Player.groundTime > 0 && (stateMachine.GetCurrentStateType() != typeof(PlayerFallingState))) return;
               

            Vector3 movementDirection = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(movementDirection);


            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            Ray ray = new Ray(stateMachine.Player.transform.position + Vector3.up, targetRotationDirection);
            Ray ray_1 = new Ray(stateMachine.Player.transform.position + Vector3.up * 0.05f, targetRotationDirection);
            Ray ray_2 = new Ray(stateMachine.Player.transform.position + Vector3.up * 0.9f, targetRotationDirection);

            stateMachine.Player.testRay = ray;
            stateMachine.Player.testRay1 = ray_1;
            stateMachine.Player.testRay2 = ray_2;

            if (Physics.Raycast(ray_2, 0.3f, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                stateMachine.Player.Rigidbody.velocity = new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
                return;
            }
            if (Physics.Raycast(ray, 0.3f, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                stateMachine.Player.Rigidbody.velocity = new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
                return;
            }
            if (!stateMachine.Player.isGround)
            {
                

                if (Physics.Raycast(ray_1, 0.3f, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
                {
                    stateMachine.Player.Rigidbody.velocity = new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
                    return;
                }
            }
            
            
            //float movementSpeed = GetMovementSpeed();
            //Debug.LogError(movementSpeed);
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();


            stateMachine.Player.Rigidbody.AddForce(targetRotationDirection * 7.5f - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);

            

            //if (!stateMachine.Player.isGround)
            //{
            //    if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) return;
            //    Vector3 heading;
            //    Vector3 moveVec;

            //    moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //    moveVec.Normalize();

            //    heading = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            //    heading.Normalize();
            //    heading = heading - moveVec;
            //    float angle = Mathf.Atan2(heading.z, heading.x) * Mathf.Rad2Deg * -2;

            //    stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * stateMachine.Player.rotateSpeed);

            //    Vector3 dir = stateMachine.Player.transform.forward * stateMachine.Player.moveSpeed * Time.deltaTime;

            //    stateMachine.Player.Rigidbody.MovePosition(stateMachine.Player.transform.position + dir);
            //    return;
            //}
            //else
            //{
            //    }
        }

        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
            stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

            stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y, ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.ReusableData.TimeToReachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            stateMachine.Player.Rigidbody.MoveRotation(targetRotation);
        }

        protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
        {
            return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            float movementSpeed = groundedData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;

            if (shouldConsiderSlopes)
            {
                movementSpeed *= stateMachine.ReusableData.MovementOnSlopesSpeedModifier;
            }

            return movementSpeed;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
        }

        protected virtual void OnContactWithGround(Collider collider)
        {

        }

        protected virtual void OnContactWithGroundExited(Collider collider)
        {
            stateMachine.Player.isGround = false;
        }

        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }

            if (movementInput == Vector2.up)
            {
                DisableCameraRecentering();

                return;
            }

            float cameraVerticalAngle = stateMachine.Player.MainCameraTransform.eulerAngles.x;

            if (cameraVerticalAngle >= 270f)
            {
                cameraVerticalAngle -= 360f;
            }

            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

            if (movementInput == Vector2.down)
            {
                SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.BackwardsCameraRecenteringData);

                return;
            }

            SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.SidewaysCameraRecenteringData);
        }

        protected void SetCameraRecenteringState(float cameraVerticalAngle, List<PlayerCameraRecenteringData> cameraRecenteringData)
        {
            foreach (PlayerCameraRecenteringData recenteringData in cameraRecenteringData)
            {
                if (!recenteringData.IsWithinRange(cameraVerticalAngle))
                {
                    continue;
                }

                EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);

                return;
            }

            DisableCameraRecentering();
        }

        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            float movementSpeed = GetMovementSpeed();

            if (movementSpeed == 0f)
            {
                movementSpeed = groundedData.BaseSpeed;
            }

            stateMachine.Player.CameraRecenteringUtility.EnableRecentering(waitTime, recenteringTime, groundedData.BaseSpeed, movementSpeed);
        }

        protected void DisableCameraRecentering()
        {
            stateMachine.Player.CameraRecenteringUtility.DisableRecentering();
        }

        protected void ResetVelocity()
        {
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
        }

        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.Rigidbody.AddForce(-playerHorizontalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            stateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }


        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }

        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minimumVelocity;
        }

        
    }
}

