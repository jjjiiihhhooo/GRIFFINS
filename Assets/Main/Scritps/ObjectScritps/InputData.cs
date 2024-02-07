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
    public KeyCode GrappleCut = KeyCode.Space;

    public KeyCode ObjectGrapple = KeyCode.Alpha2;
    public KeyCode ObjectPull = KeyCode.Mouse0;

    public KeyCode WhiteKey = KeyCode.Z;
    public KeyCode GreenKey = KeyCode.X;
    public KeyCode BlueKey = KeyCode.C;
    public KeyCode RedKey = KeyCode.V;


    public LayerMask aimColliderMask;
    public Vector3 MouseWorldPosition = Vector3.zero;
    public Transform debugTransform;
    public GameObject projectTile;

    private void Update()
    {
        KeyboardInput();
        MouseInput();
    }

    private void MouseInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            MouseWorldPosition = raycastHit.point;
        }

    }

    private void KeyboardInput()
    {

        if(Input.GetKeyDown(Catch) && !Player.Instance.isGrapple)
        {
            Player.Instance.skillData.skillIndex = 0;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
        }
        else if(Input.GetKeyDown(Throw) && !Player.Instance.isGrapple && !Player.Instance.skillData.isGHand)
        {
            Player.Instance.skillData.skillIndex = 1;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
            //Player.Instance.skillData.SendMessage(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex]);
        }
        else if(Input.GetKeyDown(Grapple) && Player.Instance.isGrapple)
        {
            Player.Instance.skillData.skillIndex = 2;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
        }
        else if (Input.GetKeyDown(Change))
        {
            if (Player.Instance.isGrapple)
            {
                GameManager.Instance.normalImage.SetActive(true);
                GameManager.Instance.grappleImage.SetActive(false);
                Player.Instance.skillData.StopGrapple();
                Player.Instance.swinging.StopSwing();
            }
            else
            {
                GameManager.Instance.grappleImage.SetActive(true);
                GameManager.Instance.normalImage.SetActive(false);
            }
            Player.Instance.isGrapple = !Player.Instance.isGrapple;
        }
        else if(Input.GetKeyDown(GrappleCut) && Player.Instance.isGrapple)
        {
            if(!Player.Instance.isGround)
            { 
                Player.Instance.skillData.StopGrapple();
                Player.Instance.swinging.StopSwing();
            }
        }
        else if(Input.GetKeyDown(ObjectGrapple) && !Player.Instance.skillData.isGHand)
        {
            Player.Instance.skillData.skillIndex = 3;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
        }
        else if(Input.GetKeyDown(ObjectPull) && Player.Instance.skillData.isGHand)
        {
            Player.Instance.skillData.skillIndex = 4;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
        }
        else if(Input.GetKeyDown(WhiteKey))
        {
            Player.Instance.ChangeCharacter(0);
        }
        else if (Input.GetKeyDown(GreenKey))
        {
            Player.Instance.ChangeCharacter(1);
        }
        else if (Input.GetKeyDown(BlueKey))
        {
            Player.Instance.ChangeCharacter(2);
        }
        else if (Input.GetKeyDown(RedKey))
        {
            Player.Instance.ChangeCharacter(3);
        }

        if (Input.GetKeyDown(Swinging) && Player.Instance.isGrapple) Player.Instance.swinging.StartSwing();
        if (Input.GetKeyUp(Swinging) && Player.Instance.isGrapple) Player.Instance.swinging.StopSwing();

        
    }
}
