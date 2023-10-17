using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        playerController.previousState = playerController.currentState;
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        playerController.IdleEnter();
    }

    public override void StateExit(PlayerController playerController)
    {

    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.Idle();
    }
}

public class PlayerWalkState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        playerController.previousState = playerController.currentState;
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        playerController.WalkEnter();
    }

    public override void StateExit(PlayerController playerController)
    {
       
    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.Walk();
    }
}

public class PlayerDashState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        playerController.previousState = playerController.currentState;
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        playerController.DashEnter();
    }

    public override void StateExit(PlayerController playerController)
    {
        playerController.DashExit();
    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.DashUpdate();
    }
}

public class PlayerSuperJumpState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        playerController.previousState = playerController.currentState;
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        Debug.LogError("dd");
        playerController.SuperJumpEnter();   
    }

    public override void StateExit(PlayerController playerController)
    {
        playerController.SuperJumpExit();
    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.SuperJumpUpdate();
    }
}
