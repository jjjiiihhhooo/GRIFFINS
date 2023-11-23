using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerLightAttackingState : PlayerAttackingState
    {
        public PlayerLightAttackingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.LightAttackParameterHash, 1);

            ResetVelocity();
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }
}
