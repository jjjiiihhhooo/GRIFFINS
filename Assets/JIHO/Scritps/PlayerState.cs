using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        Debug.Log("IdleStateChange");
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        playerController.IdleEnter();
        Debug.Log("IdleStateEnter");
    }

    public override void StateExit(PlayerController playerController)
    {
        Debug.Log("IdleStateExit");
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
        Debug.Log("WalkStateChange");
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        Debug.Log("WalkStateEnter");
        playerController.WalkEnter();
    }

    public override void StateExit(PlayerController playerController)
    {
        Debug.Log("WalkStateExit");
    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.Walk();
    }
}

public class PinBallReadyState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        Debug.Log("LandingStateChange");
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        Debug.Log("LandingStateEnter");

        playerController.PinBallReadyEnter();
    }

    public override void StateExit(PlayerController playerController)
    {
        Debug.Log("LandingStateExit");
        playerController.PinBallReadyExit();
    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.PinBallReadyUpdate();
    }
}

public class PlayerPinballState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        Debug.Log("PinballStateChange");
        playerController.currentState.StateExit(playerController);
        playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        Debug.Log("PinballStateEnter");
        playerController.PinBallEnter();
    }

    public override void StateExit(PlayerController playerController)
    {
        Debug.Log("PinballStateExit");
        playerController.PinBallExit();
    }

    public override void StateUpdate(PlayerController playerController)
    {
        playerController.PinBallUpdate();
    }
}


