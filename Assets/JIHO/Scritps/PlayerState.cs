using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
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
        //playerController.Walk();
    }
}
