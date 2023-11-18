using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace genshin
{
    public class PlayerWalkingState : PlayerGroundedState
    {
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            speedModifier = 0.225f;
        }
        #endregion

        

        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        protected void OnMovementCanceled(InputAction.CallbackContext obj)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
    }
}


