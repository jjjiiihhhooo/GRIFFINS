using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerRollingState : PlayerLandingState
{
    public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = groundedData.RollData.SpeedModifier;

        base.Enter();

        EffectActive(stateMachine.Player.landEffect, true);

        StartAnimation(stateMachine.Player.AnimationData.RollParameterHash);

        stateMachine.ReusableData.ShouldSprint = false;
    }

    public override void Exit()
    {
        base.Exit();

        EffectActive(stateMachine.Player.landEffect, false);

        StopAnimation(stateMachine.Player.AnimationData.RollParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            return;
        }

        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.MediumStoppingState);

            return;
        }

        OnMove();
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
    }
}

