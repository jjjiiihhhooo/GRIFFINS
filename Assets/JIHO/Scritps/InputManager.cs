using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    private PlayerController player;

    private bool isCursorLocked = false;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode cursorLockKey = KeyCode.LeftAlt;

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
        if (player.IsMouse) return;

        if(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            float x = Input.GetAxisRaw("Vertical");
            float z = Input.GetAxisRaw("Horizontal");

            player.MoveVec = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")));
            player.MoveVec.Normalize();

            if (player.currentState.GetType() != typeof(PlayerWalkState)) player.ChangeState(player.PlayerWalkState);
        }
        else
        {
            if (player.currentState.GetType() != typeof(PlayerIdleState)) player.ChangeState(player.PlayerIdleState);
        }

        if(Input.GetKeyDown(jumpKey))
        {
            player.IsJump = true;
        }

        if(Input.GetKeyDown(cursorLockKey))
        {
            isCursorLocked = !isCursorLocked;
            Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isCursorLocked;
        }

        if (isCursorLocked)
        {
            player.HRot = Input.GetAxis("Mouse X");
            player.IsRotate = true;
        }
        else
        {
            player.IsRotate = false;
        }
    }
}
