using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
            CoolTimeManager.Instance.GetCoolTime("Dash");

            //stateMachine.ReusableData.MovementSpeedModifier = groundedData.DashData.SpeedModifier;

            //base.Enter();

            EffectActive(stateMachine.Player.dashEffect, true);

            StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            //stateMachine.ReusableData.RotationData = groundedData.DashData.RotationData;

            Dash();

            //shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

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
            //base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            RotateTowardsTargetRotation();
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

        private void Dash()
        {
            Vector3 direction = stateMachine.Player.MainCameraTransform.forward;

            Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            Ray ray = new Ray(stateMachine.Player.transform.position, dir);
            stateMachine.Player.Rigidbody.AddForce(ray.direction * 2, ForceMode.Impulse);

            Vector3 dirY = new Vector3(ray.direction.x, 0, ray.direction.z);
            Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
            stateMachine.Player.transform.rotation = targetRotation;

            stateMachine.Player.transform.DOLocalMove(Vector3.zero, 2f).OnComplete(() =>
            {
                Debug.Log("대쉬 대기");
                stateMachine.ChangeState(stateMachine.FallingState);
                
            });


            //Vector3 dashDirection = stateMachine.Player.transform.forward;

            //dashDirection.y = 0f;

            //UpdateTargetRotation(dashDirection, false);

            //if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            //{
            //    UpdateTargetRotation(GetMovementInputDirection());

            //    dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            //}

            //stateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
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

    }
}
