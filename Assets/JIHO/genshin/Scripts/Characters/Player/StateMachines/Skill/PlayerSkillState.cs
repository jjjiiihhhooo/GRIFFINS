using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class PlayerSkillState : PlayerMovementState
    {
        public PlayerSkillState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.SkillParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.SkillParameterHash);
        }


    }
}
