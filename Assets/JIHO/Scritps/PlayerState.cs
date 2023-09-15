using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        Debug.Log("IdleStateChange");
        //playerController.currentState.StateExit(playerController);

        //playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        Debug.Log("IdleStateEnter");
    }

    public override void StateExit(PlayerController playerController)
    {
        Debug.Log("IdleStateExit");
    }

    public override void StateUpdate(PlayerController playerController)
    {
        //if (playerController.currentHelper != null)
        //{
        //    if (playerController.currentHelper.isPassive) playerController.HelperUpdate();
        //}

        //if (playerController.currentWeapon.isSightMode) return;
        //playerController.Idle();
    }
}

public class PlayerWalkState : State<PlayerController>
{
    public override void StateChange(PlayerController playerController)
    {
        Debug.Log("WalkStateChange");
        //playerController.currentState.StateExit(playerController);

        //playerController.currentState = this;

        StateEnter(playerController);
    }

    public override void StateEnter(PlayerController playerController)
    {
        Debug.Log("WalkStateEnter");
        //playerController.currentSpeed = playerController.walkSpeed;
    }

    public override void StateExit(PlayerController playerController)
    {
        Debug.Log("WalkStateExit");
        //playerController.currentSpeed = 0;
    }

    public override void StateUpdate(PlayerController playerController)
    {
        //if (playerController.currentHelper != null)
        //{
        //    if (playerController.currentHelper.isPassive) playerController.HelperUpdate();
        //}

        //if (!playerController.currentWeapon.weapon_anim.GetBool("isWalk")) playerController.currentWeapon.Walk();
    }
}

