using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerDashingState : PlayerAirborneState
    {
        private float startTime;

        private int consecutiveDashesUsed;

        private bool shouldKeepRotating;

        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            stateMachine.ReusableData.MovementSpeedModifier = groundedData.DashData.SpeedModifier;

            base.Enter();

            EffectActive(stateMachine.Player.dashEffect, true);

            stateMachine.Player.DashColActive(100);

            StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            stateMachine.ReusableData.RotationData = groundedData.DashData.RotationData;

            Dash();

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            //UpdateConsecutiveDashes();

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.DashColActive();
            EffectActive(stateMachine.Player.dashEffect, false);

            StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            // base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            //RotateTowardsTargetRotation();
        }

       
        public override void OnAnimationTransitionEvent()
        {
            shouldGroundChecking = true;


            stateMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Maximum;
            stateMachine.Player.groundTime = stateMachine.Player.groundMaxTime;
            
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }

        private void Dash()
        {
            shouldGroundChecking = false;

            Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            stateMachine.Player.ray = new Ray(stateMachine.Player.transform.position, dir);
            
            stateMachine.Player.Rigidbody.AddForce(stateMachine.Player.ray.direction * stateMachine.Player.dashSpeed, ForceMode.Impulse);

            Vector3 dirY = new Vector3(stateMachine.Player.ray.direction.x, 0, stateMachine.Player.ray.direction.z);
            Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
            stateMachine.Player.transform.rotation = targetRotation;

            //stateMachine.Player.InvokeMessage(0.3f);
        }


        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if (consecutiveDashesUsed == groundedData.DashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;

                stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash, groundedData.DashData.DashLimitReachedCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + groundedData.DashData.TimeToBeConsideredConsecutive;
        }


        protected override void OnContactWithGround(Collider collider)
        {
            if (!shouldGroundChecking) return;

            Ray ray = new Ray(stateMachine.Player.transform.position + Vector3.up, stateMachine.Player.transform.forward);
            stateMachine.Player.testRay = ray;

            if (Physics.Raycast(ray, 0.3f, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                stateMachine.Player.groundTime = stateMachine.Player.groundMaxTime;
                return;
            }

            stateMachine.ChangeState(stateMachine.LightLandingState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);
        }
    }
}
