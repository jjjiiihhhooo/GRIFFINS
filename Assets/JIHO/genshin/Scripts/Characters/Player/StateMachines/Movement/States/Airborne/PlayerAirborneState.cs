using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
            stateMachine.Player.Input.PlayerActions.DownStream.started += OnDownStreamStarted;
            stateMachine.Player.Input.PlayerActions.Tornado.started += OnTornadoStarted;
        }

        private void OnTornadoStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.TornadoState);
        }

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            if (!CoolTimeManager.instance.CoolCheck("Dash")) return;

            CoolTimeManager.instance.GetCoolTime("Dash");

            stateMachine.ChangeState(stateMachine.DashingState);
        }

        protected virtual void OnDownStreamStarted(InputAction.CallbackContext context)
        {
            if (!CoolTimeManager.instance.CoolCheck("DownStream")) return;

            CoolTimeManager.instance.GetCoolTime("DownStream");

            stateMachine.ChangeState(stateMachine.DownStreamState);
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
            stateMachine.Player.Input.PlayerActions.DownStream.started -= OnDownStreamStarted;
            stateMachine.Player.Input.PlayerActions.Tornado.started -= OnTornadoStarted;
        }

        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }

        protected override void OnContactWithGround(Collider collider)
        {
            if (stateMachine.GetCurrentStateType() != typeof(PlayerDownStreamState))
            {
                stateMachine.ChangeState(stateMachine.LightLandingState);
            }
        }
    }
}
