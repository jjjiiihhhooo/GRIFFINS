using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private float startTime;

        private int consecutiveDashesUsed;

        private bool shouldKeepRotating;

        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = groundedData.DashData.SpeedModifier;

            base.Enter();

            EffectActive(stateMachine.Player.dashEffect, true);

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
            //if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            //{
            //    stateMachine.ChangeState(stateMachine.HardStoppingState);

            //    return;
            //}

            //stateMachine.ChangeState(stateMachine.SprintingState);
        }

        //protected override void AddInputActionsCallbacks()
        //{
        //    base.AddInputActionsCallbacks();

        //    stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;

        //}

        //protected override void RemoveInputActionsCallbacks()
        //{
        //    base.RemoveInputActionsCallbacks();

        //    stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        //}

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
            Vector3 pos = stateMachine.Player.transform.position;
            stateMachine.Player.Rigidbody.AddForce(stateMachine.Player.ray.direction * 30f, ForceMode.Impulse);
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

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.LightLandingState);
        }
    }
}
