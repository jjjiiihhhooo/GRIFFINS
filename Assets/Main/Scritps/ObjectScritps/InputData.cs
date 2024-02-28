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

    public KeyCode ObjectGrapple = KeyCode.Alpha2;
    public KeyCode ObjectPull = KeyCode.Mouse0;

    public KeyCode InteractionKey = KeyCode.F;
    public KeyCode scene_assistKey = KeyCode.Tab;

    public KeyCode WhiteKey = KeyCode.Z;
    public KeyCode GreenKey = KeyCode.X;
    public KeyCode BlueKey = KeyCode.C;
    public KeyCode RedKey = KeyCode.V;

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
        AttackInput();
        PsycheInput();
        GrapleInput();
        ChangeInput();
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("DownInteract");
            player.currentCharacter.Interaction();
        }
    }

    
    private void AttackInput()
    {
        if (!GameManager.Instance.tutorialManager.attack) return;

        if (Input.GetKeyDown(Attack) && !player.isGrapple && !player.isPsyche)
        {
            player.currentCharacter.NormalAttack();
        }
    }

    private void PsycheInput()
    {
        if (!GameManager.Instance.tutorialManager.psyche) return;
        if (player.isAttack) return;
        if (player.isGrapple) return;
        if (Input.GetKeyDown(Catch))
        {
            player.currentCharacter.Catch();
        }
        else if (Input.GetKeyDown(Throw) && player.isPsyche)
        {
            player.currentCharacter.Throw();
        }
        else if (Input.GetKeyDown(ObjectGrapple) && !player.skillData.isGHand)
        {
            player.currentCharacter.GCatch();
        }
        else if (Input.GetKeyDown(ObjectPull) && player.skillData.isGHand)
        {
            player.currentCharacter.GPull();
        }
    }

    private void GrapleInput()
    {
        if (!GameManager.Instance.tutorialManager.graple) return;
        if (player.isAttack) return;
        if (!player.isGrapple) return;

        if (Input.GetKeyDown(Grapple))
        {
            player.currentCharacter.StartGrapple();
        }
        else if (Input.GetKeyDown(GrappleCut) && !player.isGround)
        {
            player.currentCharacter.StopGrapple();
            player.swinging.StopSwing();
        }

        if (Input.GetKeyDown(Swinging)) player.swinging.StartSwing();
        if (Input.GetKeyUp(Swinging)) player.swinging.StopSwing();

    }

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
        else if (Input.GetKeyDown(BlueKey))
        {
            player.ChangeCharacter(2);
        }
        else if (Input.GetKeyDown(RedKey))
        {
            player.ChangeCharacter(3);
        }

        if (GameManager.Instance.tutorialManager.skillIndex <= 1) return;

        if (Input.GetKeyDown(Change) && !player.isAttack)
        {
            if (player.isGrapple)
            {
                GameManager.Instance.normalImage.SetActive(true);
                GameManager.Instance.grappleImage.SetActive(false);
                player.currentCharacter.StopGrapple();
                player.swinging.StopSwing();
            }
            else
            {
                GameManager.Instance.grappleImage.SetActive(true);
                GameManager.Instance.normalImage.SetActive(false);
            }
            player.isGrapple = !player.isGrapple;
        }
    }
}
