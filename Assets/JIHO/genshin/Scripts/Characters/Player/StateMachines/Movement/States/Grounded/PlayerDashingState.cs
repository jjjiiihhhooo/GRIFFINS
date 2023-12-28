using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            //if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            //{
            //    stateMachine.ChangeState(stateMachine.HardStoppingState);

            //    return;
            //}

            //stateMachine.ChangeState(stateMachine.SprintingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }

        //protected override void OnMovementPerformed(InputAction.CallbackContext context)
        //{
        //    base.OnMovementPerformed(context);

        //    shouldKeepRotating = true;
        //}

        private void Dash()
        {
            //Vector3 dashDirection = stateMachine.Player.transform.forward;

            //dashDirection.y = 0f;

            //UpdateTargetRotation(dashDirection, false);

            //if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            //{
            //    UpdateTargetRotation(GetMovementInputDirection());

            //    dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            //}

            //stateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);

            //Vector3 dir = stateMachine.Player.dir;
            shouldGroundChecking = false;


            Vector3 pos = stateMachine.Player.transform.position;
            Vector3 dir = stateMachine.Player.ray.direction;

            if (stateMachine.Player.isGround)
            {
                dir = new Vector3(dir.x, dir.y + 0.6f, dir.z);
            }

            stateMachine.Player.Rigidbody.AddForce(dir * stateMachine.ReusableData.MovementSpeedModifier, ForceMode.VelocityChange);
            
            Vector3 dirY = new Vector3(stateMachine.Player.ray.direction.x, 0, stateMachine.Player.ray.direction.z);
            Quaternion targetRot = Quaternion.LookRotation(dirY, Vector3.up);
            stateMachine.Player.transform.rotation = targetRot;

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

            stateMachine.ChangeState(stateMachine.LightLandingState);
        }
    }
}
