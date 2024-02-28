using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

        stateMachine.ReusableData.MovementDecelerationForce = groundedData.StopData.HardDecelerationForce;

        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);
    }

    protected override void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }
}

