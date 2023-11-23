using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerAttackingState : PlayerGroundedState
    {
        public PlayerAttackingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        }
    }
}
