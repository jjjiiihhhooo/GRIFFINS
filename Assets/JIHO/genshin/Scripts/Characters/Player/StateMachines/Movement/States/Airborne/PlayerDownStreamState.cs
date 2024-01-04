using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerDownStreamState : PlayerAirborneState
    {
        public PlayerDownStreamState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            EffectActive(stateMachine.Player.jumpEffect, true);

            StartAnimation(stateMachine.Player.AnimationData.DownStreamParameterHash);

            //stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.MovementDecelerationForce = airborneData.JumpData.DecelerationForce;

            stateMachine.ReusableData.RotationData = airborneData.JumpData.RotationData;

            DownStream();

        }

        public override void Exit()
        {
            base.Exit();

            EffectActive(stateMachine.Player.jumpEffect, false);

            StopAnimation(stateMachine.Player.AnimationData.DownStreamParameterHash);

            SetBaseRotationData();

        }

        public override void Update()
        {
            base.Update();

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void DownStream()
        {
            stateMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Maximum;
            stateMachine.Player.groundTime = stateMachine.Player.groundMaxTime;

            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            stateMachine.Player.Rigidbody.AddForce(Vector3.down * stateMachine.Player.Data.AirborneData.DownStreamData.SpeedModifier, ForceMode.VelocityChange);
        }

        protected override void OnContactWithGround(Collider collider)
        {
            
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {

            stateMachine.Player.StartCor(DelayCor());
        }
        
        private IEnumerator DelayCor()
        {
            yield return new WaitForSeconds(0.2f);
            stateMachine.ChangeState(stateMachine.FallingState);
        }
    }
}
