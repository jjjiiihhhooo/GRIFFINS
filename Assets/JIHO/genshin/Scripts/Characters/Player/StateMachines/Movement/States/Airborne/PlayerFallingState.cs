using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private Vector3 playerPositionOnEnter;

        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
            playerPositionOnEnter = stateMachine.Player.transform.position;

            //stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            //if (stateMachine.GetPreviousState() != typeof(PlayerDownStreamState))
            //{
            //    stateMachine.ReusableData.MovementSpeedModifier = 0f;

            //    ResetVerticalVelocity();
            //}
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            //LimitVerticalVelocity();
        }

        public override void Update()
        {
            
        }


        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -airborneData.FallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocityForce = new Vector3(stateMachine.Player.Rigidbody.velocity.x, -airborneData.FallData.FallSpeedLimit - playerVerticalVelocity.y, stateMachine.Player.Rigidbody.velocity.z);

            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce * 1.2f, ForceMode.VelocityChange);
            }
            else
            {
                stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
            }

            
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.Player.isGround = true;
            float fallDistance = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;

            if (fallDistance < airborneData.FallData.MinimumDistanceToBeConsideredHardFall)
            {
                stateMachine.ChangeState(stateMachine.LightLandingState);

                return;
            }

            if (stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint || stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardLandingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.RollingState);

        }
    }
}
