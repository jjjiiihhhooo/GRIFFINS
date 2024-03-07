using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        EffectActive(stateMachine.Player.landEffect, true);

        StartAnimation(stateMachine.Player.AnimationData.LandingParameterHash);

        DisableCameraRecentering();
    }

    public override void Exit()
    {
        base.Exit();

        EffectActive(stateMachine.Player.landEffect, false);

        StopAnimation(stateMachine.Player.AnimationData.LandingParameterHash);
    }
}

