using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace genshin
{
    public class ObjectDashState : PlayerAirborneState
    {
        public ObjectDashState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            EffectActive(stateMachine.Player.dashEffect, true);

            stateMachine.Player.DashColActive(100);

            Dash();

            StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);


        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.DashColActive();
            EffectActive(stateMachine.Player.dashEffect, false);

            StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);

            //SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {


        }

        public override void OnAnimationTransitionEvent()
        {
            shouldGroundChecking = true;
            stateMachine.Player.groundTime = stateMachine.Player.groundMaxTime;
        }


        private void Dash()
        {
            shouldGroundChecking = false;

            stateMachine.Player.pm.bounceCombine = PhysicMaterialCombine.Maximum;
            stateMachine.Player.groundTime = stateMachine.Player.groundMaxTime;

            Ray ray = new Ray(stateMachine.Player.transform.position, airborneData.ObjectDashData.Direction.normalized);

            stateMachine.Player.Rigidbody.AddForce(ray.direction * airborneData.ObjectDashData.SpeedModifier, ForceMode.VelocityChange);

            Vector3 dirY = new Vector3(airborneData.ObjectDashData.Direction.x, 0, airborneData.ObjectDashData.Direction.z);
            Quaternion targetRot = Quaternion.LookRotation(dirY, Vector3.up);
            stateMachine.Player.transform.rotation = targetRot;

        }


        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            if (!shouldGroundChecking) return;

            //stateMachine.Player.transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(() => { });
            stateMachine.ChangeState(stateMachine.LightLandingState);
        }
    }
}
