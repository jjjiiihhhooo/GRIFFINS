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

            stateMachine.ReusableData.MovementSpeedModifier = 0f;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void OnAnimationTransitionEvent()
        {
            
        }
    }
}
