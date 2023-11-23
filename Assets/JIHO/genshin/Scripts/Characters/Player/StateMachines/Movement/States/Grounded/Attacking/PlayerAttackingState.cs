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

            //EffectActive(stateMachine.Player.landEffect, true);

            StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash,1);

        }

        public override void Exit()
        {
            base.Exit();

            //EffectActive(stateMachine.Player.landEffect, false);

            //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash,);
        }
    }
}
