using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private bool shouldKeepRotating;
        private bool canStartFalling;

        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            EffectActive(stateMachine.Player.jumpEffect, true);

            //stateMachine.ReusableData.MovementSpeedModifier = 0f;

            //stateMachine.ReusableData.MovementDecelerationForce = airborneData.JumpData.DecelerationForce;

            //stateMachine.ReusableData.RotationData = airborneData.JumpData.RotationData;

            //shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            Jump();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Exit()
        {
            base.Exit();

            EffectActive(stateMachine.Player.jumpEffect, false);

            //SetBaseRotationData();

            canStartFalling = false;
        }

        public override void Update()
        {
            //base.Update();
            
            //if (!canStartFalling && IsMovingUp(0f))
            //{
            //    canStartFalling = true;
            //}

            //if (!canStartFalling || IsMovingUp(0f))
            //{
            //    return;
            //}

            //stateMachine.ChangeState(stateMachine.FallingState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }

        private void Jump()
        {
            //stateMachine.Player.Rigidbody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
            
            //Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

            //Vector3 jumpDirection = stateMachine.Player.transform.forward;

            //if (shouldKeepRotating)
            //{
            //    UpdateTargetRotation(GetMovementInputDirection());

            //    jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            //}

            //jumpForce.x = 0f;
            //jumpForce.z = 0f;

            //jumpForce = GetJumpForceOnSlope(jumpForce);
            
            //ResetVelocity();

            stateMachine.Player.Rigidbody.AddForce(Vector3.up * stateMachine.Player.jumpSpeed, ForceMode.VelocityChange);

            stateMachine.Player.InvokeMessage(0.1f);
        }



        private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, airborneData.JumpData.JumpToGroundRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = airborneData.JumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= 0;
                    jumpForce.z *= 0;
                }

                if (IsMovingDown())
                {
                    float forceModifier = airborneData.JumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            return jumpForce;
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);
        }

    }
}
