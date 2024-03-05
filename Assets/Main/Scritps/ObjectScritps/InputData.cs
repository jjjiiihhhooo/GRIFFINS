using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputData : MonoBehaviour
{
    public KeyCode Catch = KeyCode.Mouse1;
    public KeyCode Throw = KeyCode.Mouse0;
    public KeyCode Grapple = KeyCode.Mouse0;
    public KeyCode Swinging = KeyCode.Mouse1;
    public KeyCode Change = KeyCode.Alpha1;
    public KeyCode Attack = KeyCode.Mouse0;
    public KeyCode GrappleCut = KeyCode.Space;

    public KeyCode LeftAction = KeyCode.Mouse0;
    public KeyCode RightAction = KeyCode.Mouse1;

    public KeyCode ObjectGrapple = KeyCode.Alpha2;
    public KeyCode ObjectPull = KeyCode.Mouse0;

    public KeyCode InteractionKey = KeyCode.F;
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
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            MouseWorldPosition = raycastHit.point;
        }

    }

    private void KeyboardInput()
    {
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

    private void PsycheInput()
    {
        //if (!GameManager.Instance.tutorialManager.psyche) return;
        //if (player.isAttack) return;
        //if (player.isGrapple) return;
        //if (Input.GetKeyDown(Catch))
        //{
        //    player.currentCharacter.Catch();
        //}
        //else if (Input.GetKeyDown(Throw) && player.isPsyche)
        //{
        //    player.currentCharacter.Throw();
        //}
        //else if (Input.GetKeyDown(ObjectGrapple) && !player.skillData.isGHand)
        //{
        //    player.currentCharacter.GCatch();
        //}
        //else if (Input.GetKeyDown(ObjectPull) && player.skillData.isGHand)
        //{
        //    player.currentCharacter.GPull();
        //}
    }

    //private void GrapleInput()
    //{
    //    if (!GameManager.Instance.tutorialManager.graple) return;
    //    if (player.isAttack) return;
    //    if (!player.isGrapple) return;

    //    if (Input.GetKeyDown(Grapple))
    //    {
    //        player.currentCharacter.StartGrapple();
    //    }
    //    else if (Input.GetKeyDown(GrappleCut) && !player.isGround)
    //    {
    //        player.currentCharacter.StopGrapple();
    //        player.swinging.StopSwing();
    //    }

    //    if (Input.GetKeyDown(Swinging)) player.swinging.StartSwing();
    //    if (Input.GetKeyUp(Swinging)) player.swinging.StopSwing();

    //}

    private void ChangeInput()
    {
        if (Input.GetKeyDown(WhiteKey))
        {
            player.ChangeCharacter(0);
        }
        else if (Input.GetKeyDown(GreenKey))
        {
            player.ChangeCharacter(1);
        }
        else if (Input.GetKeyDown(RedKey))
        {
            player.ChangeCharacter(2);
        }

        //if (GameManager.Instance.tutorialManager.skillIndex <= 1) return;

        //if (Input.GetKeyDown(Change) && !player.isAttack)
        //{
        //    if (player.isGrapple)
        //    {
        //        GameManager.Instance.normalImage.SetActive(true);
        //        GameManager.Instance.grappleImage.SetActive(false);
        //        player.currentCharacter.StopGrapple();
        //        player.swinging.StopSwing();
        //    }
        //    else
        //    {
        //        GameManager.Instance.grappleImage.SetActive(true);
        //        GameManager.Instance.normalImage.SetActive(false);
        //    }
        //    player.isGrapple = !player.isGrapple;
        //}
    }
}
