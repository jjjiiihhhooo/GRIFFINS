using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);

            UpdateShouldSprintState();

            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        private void UpdateShouldSprintState()
        {
            if (!stateMachine.ReusableData.ShouldSprint)
            {
                return;
            }

            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            stateMachine.ReusableData.ShouldSprint = false;
        }

        private void Float()
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, stateMachine.Player.ResizableCapsuleCollider.SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                {
                    return;
                }

                float distanceToFloatingPoint = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * stateMachine.Player.ResizableCapsuleCollider.SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = groundedData.SlopeSpeedAngles.Evaluate(angle);

            if (stateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

                UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
            }

            return slopeSpeedModifier;
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;

            stateMachine.Player.Input.PlayerActions.LightAttack.started += OnAttackStarted;

            stateMachine.Player.Input.PlayerActions.DownStream.started += OnDownStreamStarted;

            stateMachine.Player.Input.PlayerActions.Tornado.started += OnTornadoStarted;

            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
        }

        private void OnDashStarted(InputAction.CallbackContext context)
        {
            if (!CoolTimeManager.instance.CoolCheck("Dash")) return;

            CoolTimeManager.instance.GetCoolTime("Dash");

            stateMachine.ChangeState(stateMachine.DashingState);
        }

        protected virtual void OnTornadoStarted(InputAction.CallbackContext context)
        {
            if (!CoolTimeManager.instance.CoolCheck("Tornado")) return;

            CoolTimeManager.instance.GetCoolTime("Tornado");

            stateMachine.ChangeState(stateMachine.TornadoState);
        }

        protected virtual void OnDownStreamStarted(InputAction.CallbackContext context)
        {
            if (!CoolTimeManager.instance.CoolCheck("DownStream")) return;

            CoolTimeManager.instance.GetCoolTime("DownStream");

            stateMachine.ChangeState(stateMachine.DownStreamState);
        }

        protected virtual void OnAttackStarted(InputAction.CallbackContext context)
        {
            if(stateMachine.GetCurrentStateType() == typeof(PlayerDashingState))
            {
                return;
            }

            if (stateMachine.GetCurrentStateType() == typeof(PlayerLightAttackingState))
            {
                StartAnimation(stateMachine.Player.AnimationData.LightAttackParameterHash, 1);
                return;
            }
            stateMachine.ChangeState(stateMachine.LightAttackingState);
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;

            stateMachine.Player.Input.PlayerActions.LightAttack.started -= OnAttackStarted;

            stateMachine.Player.Input.PlayerActions.DownStream.started -= OnDownStreamStarted;

            stateMachine.Player.Input.PlayerActions.Tornado.started -= OnTornadoStarted;

            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            //if (stateMachine.GetCurrentStateType() == typeof(PlayerLightAttackingState))
            //{
            //    return;
            //}

            stateMachine.ChangeState(stateMachine.JumpingState);
        }

        protected virtual void OnMove()
        {
            if (stateMachine.GetPreviousState() == typeof(PlayerDashingState))
            {
                return;
            }

            if (stateMachine.ReusableData.ShouldSprint)
            {
                stateMachine.ChangeState(stateMachine.SprintingState);

                return;
            }

            if (stateMachine.ReusableData.ShouldWalk)
            {
                stateMachine.ChangeState(stateMachine.WalkingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, groundedData.GroundToFallRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        private bool IsThereGroundUnderneath()
        {
            PlayerTriggerColliderData triggerColliderData = stateMachine.Player.ResizableCapsuleCollider.TriggerColliderData;

            Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }

        protected virtual void OnFall()
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);

            UpdateTargetRotation(GetMovementInputDirection());
        }
    }
}
