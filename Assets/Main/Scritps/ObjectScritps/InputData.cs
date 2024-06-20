
using UnityEngine;
using UnityEngine.InputSystem;

public class InputData : MonoBehaviour
{
    public KeyCode LeftAction = KeyCode.Mouse0;
    public KeyCode RightAction = KeyCode.Mouse1;

    public KeyCode Q = KeyCode.Q;
    public KeyCode E = KeyCode.E;

    public KeyCode InteractionKey = KeyCode.F;

    public KeyCode WhiteKey = KeyCode.Alpha1;
    public KeyCode GreenKey = KeyCode.Alpha2;
    public KeyCode RedKey = KeyCode.Alpha3;

    public LayerMask aimColliderMask;
    public Vector3 MouseWorldPosition = Vector3.zero;

    private Player player;

    public void Init()
    {

    }

    private void Update()
    {
        if (player == null) player = Player.Instance;
        if (GameManager.Instance.isCutScene) return;
        if (player.isDead) return;
        MenuInput();

        KeyboardInput();
        MouseInput();
    }

    private void MenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Menu();
        }
    }

    private void MouseInput()
    {
        if (player == null) return;
        if (GameManager.Instance.isMenu) return;
        if (GameManager.Instance.dialogueManager.IsChat) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            MouseWorldPosition = raycastHit.point;
        }

    }

    private void KeyboardInput()
    {
        if (player == null) return;
        ActionInput();
        ChangeInput();
        InteractInput();
    }

    private void ActionInput()
    {
        if (GameManager.Instance.dialogueManager.IsChat) return;
        if (Player.Instance.currentCharacter.isGrappleReady) return;
        if (!Player.Instance.isGround) return;
        if (Player.Instance.isAttack) return;
        if (Player.Instance.isNormalAttack) return;

        if (Input.GetKeyDown(LeftAction))
        {
            player.currentCharacter.LeftAction();
        }

        if (Input.GetKeyDown(Q))
        {
            player.currentCharacter.Q_Action();
        }
        else if (Input.GetKeyDown(E))
        {
            player.currentCharacter.E_Action();
        }

        //if (Input.GetKeyDown(R))
        //{
        //    player.currentCharacter.R_Action();
        //}
    }

    private void InteractInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.dialogueManager.CurDialogues != null)
            {
                GameManager.Instance.dialogueManager.StartDialogue();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.currentCharacter.Interaction();
        }
    }

    private void ChangeInput()
    {
        //if (!GameManager.Instance.tutorialManager.characterChange) return;
        if (GameManager.Instance.dialogueManager.IsChat) return;
        if (Player.Instance.currentCharacter.isGrappleReady) return;
        if (Player.Instance.isAttack) return;
        if (Player.Instance.isNormalAttack) return;

        if (Input.GetKeyDown(WhiteKey))
        {

            Debug.Log("whiteChange");

            player.ChangeCharacter(0);

        }
        else if (Input.GetKeyDown(GreenKey))
        {

            Debug.Log("greenChange");

            player.ChangeCharacter(1);
        }
        else if (Input.GetKeyDown(RedKey))
        {
            Debug.Log("redChange");
            player.ChangeCharacter(2);
        }


    }
}
