using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        if (player == null)
        {
            player = PlayerController.Instance;
        }
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (player.IsMouse) return;

        if(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (player.currentState.GetType() == typeof(PinBallReadyState)) return;
            if (player.currentState.GetType() == typeof(PlayerPinballState)) return;

            if (player.currentState.GetType() != typeof(PlayerWalkState)) player.ChangeState(player.PlayerWalkState);
        }
        else
        {
            if (player.currentState.GetType() == typeof(PinBallReadyState)) return;
            if (player.currentState.GetType() == typeof(PlayerPinballState)) return;

            if (player.currentState.GetType() != typeof(PlayerIdleState)) player.ChangeState(player.PlayerIdleState);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (player.currentState.GetType() != typeof(PinBallReadyState)) 
            {
                player.ChangeState(player.PinBallReadyState);
            }
            else
            {
                player.PinBallReadyCancel();
                player.ChangeState(player.PlayerIdleState);
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (player.currentState.GetType() == typeof(PinBallReadyState)) player.ChangeState(player.PlayerPinballState);
        }
    }
}
