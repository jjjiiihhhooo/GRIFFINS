
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
            int temp = player.currentUnit.animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            if (player.currentUnit.currentState.GetType() != typeof(PlayerWalkState) && temp == 0)
            {
                if(player.currentUnit.animator.GetCurrentAnimatorStateInfo(1).IsName("AttackIdle"))
                {
                    player.currentUnit.animator.SetLayerWeight(1, 0f);
                    player.currentUnit.animator.SetLayerWeight(0, 1f);

                }

                player.ChangeState(player.PlayerWalkState);
            }
        }
        else
        {
            if (player.currentUnit.currentState.GetType() != typeof(PlayerIdleState)) player.ChangeState(player.PlayerIdleState);
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

        if(Input.GetKeyDown(dashKey))
        {
            Debug.Log("dd");
            player.Dash();
        }

        if(Input.GetMouseButtonDown(0))
        {
            player.BasicAttack();
        }

        if(Input.GetMouseButtonDown(1))
        {
            player.SuperJump();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.ChangeUnit(player.WhiteUnit);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.ChangeUnit(player.RedUnit);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.ChangeUnit(player.GreenUnit);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.ChangeUnit(player.BlueUnit);
        }
    }
}
