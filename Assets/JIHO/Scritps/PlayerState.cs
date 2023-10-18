using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        playerController.animator.SetBool("Walk", false);
        playerController.IsMove = false;
    }

    public override void StateExit(PlayerController playerController)
    {

    }

    public override void StateUpdate(PlayerController playerController)
    {
        
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
        playerController.animator.SetBool("Walk", true);
        playerController.IsMove = true;
    }

    public override void StateExit(PlayerController playerController)
    {
       
    }

    public override void StateUpdate(PlayerController playerController)
    {
        //Vector3 move = playerController.MoveVec;
        //Vector3 heading = playerController.Heading;


        //move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //move.Normalize();

        //heading = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        //heading.Normalize();

        //heading = heading - move;

        //float angle = Mathf.Atan2(heading.z, heading.x) * Mathf.Rad2Deg * -2;

        //playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, Quaternion.Euler(0, angle, 0), Time.fixedDeltaTime * playerController.RotateSpeed);

        //Vector3 dir = playerController.transform.forward * playerController.MoveSpeed * Time.fixedDeltaTime;

        //playerController.rigid.MovePosition(playerController.transform.position + dir);
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
        
    }

    public override void StateExit(PlayerController playerController)
    {
        
    }

    public override void StateUpdate(PlayerController playerController)
    {
        
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
        
    }

    public override void StateExit(PlayerController playerController)
    {

    }

    public override void StateUpdate(PlayerController playerController)
    {

    }
}
