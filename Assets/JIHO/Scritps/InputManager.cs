using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    private PlayerController player;

    private bool isCursorLocked = false;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode cursorLockKey = KeyCode.LeftAlt;
    [SerializeField] private KeyCode superJumpKey = KeyCode.Mouse1;

    public bool IsCursorLocked { get => isCursorLocked; }
    
    private void Start()
    {
        if (player == null)
        {
            player = PlayerController.Instance;
        }

        isCursorLocked = false;
        
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (player.currentState.GetType() != typeof(PlayerWalkState)) player.ChangeState(player.PlayerWalkState);
        }
        else
        {
            if (player.currentState.GetType() != typeof(PlayerIdleState)) player.ChangeState(player.PlayerIdleState);
        }

        if (Input.GetKeyDown(jumpKey))
        {
            player.Jump();
        }

        if(Input.GetKeyDown(cursorLockKey))
        {
            isCursorLocked = !isCursorLocked;
            Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isCursorLocked;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("dd");
            player.Dash();
        }

        if(Input.GetMouseButtonDown(1))
        {
            player.SuperJump();
        }

    }
}
