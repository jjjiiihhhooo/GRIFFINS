using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace genshin
{
    public class PlayerDownStreamState : PlayerAirborneState
    {
        public PlayerDownStreamState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public bool isDown;

        private Vector3 playerPositionOnEnter;


        public override void Enter()
        {
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            stateMachine.Player.Rigidbody.useGravity = false;

            stateMachine.ReusableData.MovementSpeedModifier = groundedData.UpStreamData.SpeedModifier;

            playerPositionOnEnter = stateMachine.Player.transform.position;

            base.Enter();

            //EffectActive(stateMachine.Player.dashEffect, true);

            StartAnimation(stateMachine.Player.AnimationData.DownStreamParameterHash);

            //stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            //stateMachine.ReusableData.RotationData = groundedData.DashData.RotationData;

        }

        public override void Exit()
        {
            StopAnimation(stateMachine.Player.AnimationData.DownStreamParameterHash);
            base.Exit();

            //EffectActive(stateMachine.Player.dashEffect, false);


            //SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            //base.PhysicsUpdate();

            DowmStream();
        }

        private void DowmStream()
        {
            if (!isDown) return;

            //Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            //if (playerVerticalVelocity.y >= -airborneData.DownStreamData.FallSpeedLimit)
            //{
            //    return;
            //}

            Vector3 limitedVelocityForce = new Vector3(0f, -airborneData.DownStreamData.FallSpeedLimit, 0f);

            stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);

        }

        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.Player.Data.GroundedData.UpStreamData.fallCount = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;

            

            isDown = false;

            stateMachine.ChangeState(stateMachine.UpStreamState);

        }

        private void SetDownStream()
        {
            stateMachine.Player.Rigidbody.useGravity = true;
            isDown = true;
            Debug.LogError("IsDown");
        }

        public override void OnAnimationEnterEvent()
        {
            
        }

        public override void OnAnimationTransitionEvent()
        {
            SetDownStream();
            //stateMachine.ChangeState(stateMachine.FallingState);
        }
    }
}
