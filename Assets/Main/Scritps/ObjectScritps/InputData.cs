using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputData : MonoBehaviour
{
    public KeyCode GrappleCut = KeyCode.Space;

    public KeyCode LeftAction = KeyCode.Mouse0;
    public KeyCode RightAction = KeyCode.Mouse1;

    public KeyCode InteractionKey = KeyCode.F;
    public KeyCode itemSaveKey = KeyCode.Alpha5;
    public KeyCode scene_assistKey = KeyCode.Tab;

    public KeyCode WhiteKey = KeyCode.Alpha1;
    public KeyCode GreenKey = KeyCode.Alpha2;
    public KeyCode RedKey = KeyCode.Alpha3;

    public LayerMask aimColliderMask;
    public Vector3 MouseWorldPosition = Vector3.zero;
    public GameObject projectTile;
    private Player player;


    public void Init()
    {

    }
    private void Update()
    {
        if (player == null) player = Player.Instance;
        KeyboardInput();
        MouseInput();
    }

    private void MouseInput()
    {
        if (player == null) return;
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
        if(Input.GetKeyDown(LeftAction))
        {
            player.currentCharacter.LeftAction();
        }

        if(Input.GetKeyDown(RightAction) && player.currentCharacter.GetType() != typeof(GreenCharacter))
        {
            player.currentCharacter.RightAction();
        }
        
        if(Input.GetKeyDown(RightAction) && player.currentCharacter.GetType() == typeof(GreenCharacter))
        {
            player.swinging.StartSwing();
        }

        if(Input.GetKeyUp(RightAction) && player.currentCharacter.GetType() == typeof(GreenCharacter))
        {
            player.swinging.StopSwing();
        }

        if(Input.GetKeyDown(itemSaveKey))
        {
            player.currentCharacter.ItemSave();
        }
    }

    private void InteractInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.dialogueManager.CurDialogues == null) return;
            GameManager.Instance.dialogueManager.StartDialogue();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.currentCharacter.Interaction();
        }
    }


    private void ChangeInput()
    {
        

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
