using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerUpStreamState : PlayerGroundedState
    {
        public PlayerUpStreamState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public float maxHeight = 50f;

        public Vector3 pos;


        public override void Enter()
        {
            //stateMachine.Player.Rigidbody.useGravity = false;
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            stateMachine.ReusableData.MovementSpeedModifier = groundedData.UpStreamData.SpeedModifier;

            pos = stateMachine.Player.transform.position;

            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.UpStreamParameterHash);

            //EffectActive(stateMachine.Player.dashEffect, true);



        }

        public override void Exit()
        {
            StopAnimation(stateMachine.Player.AnimationData.UpStreamParameterHash);
            base.Exit();

            //EffectActive(stateMachine.Player.dashEffect, false);


            //SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            //base.PhysicsUpdate();
            UpStream();
        }

        private void UpStream()
        {
            
            float tempHeight = stateMachine.Player.Data.GroundedData.UpStreamData.fallCount * 2;

            //Debug.LogError("dd : " + tempHeight);
            float temp = stateMachine.Player.transform.position.y - pos.y;
            //Debug.LogError("pos : " + temp);
            if (tempHeight > maxHeight)
            {
                tempHeight = maxHeight;
            }

            if (stateMachine.Player.transform.position.y - pos.y > tempHeight)
            {
                //stateMachine.Player.Rigidbody.useGravity = true;
                stateMachine.ChangeState(stateMachine.FallingState);
                return;
            }

            //Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            Vector3 limitedVelocityForce = new Vector3(0f, groundedData.UpStreamData.upPower, 0f);

            stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }

        public override void OnAnimationTransitionEvent()
        {
            //stateMachine.ChangeState(stateMachine.FallingState);
        }

        protected override void OnContactWithGround(Collider collider)
        {

        }

    }
}
