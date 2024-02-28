using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementDecelerationForce = groundedData.StopData.LightDecelerationForce;

        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
    }
}

