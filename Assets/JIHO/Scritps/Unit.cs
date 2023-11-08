using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public class Unit<T>
{

    public State<T> currentState;
    public Animator animator;
    public GameObject unit_obj;
    public GameObject attackCol;
    public GameObject changeEffect_obj;

    public float curDamage;
    public float normalDamage;
    public float skillDamage;


    public virtual void ChangeUnit(T PlayerController)
    {
        
    }

    public virtual void Enter(T PlayerController)
    {

    }

    public virtual void Exit(T PlayerController)
    {

    }

    public virtual void AttackAction(T PlayerController)
    {

    }

    public virtual void SkillAction(T PlayerController)
    {

    }

    public virtual void Dash(T PlayerController)
    {
        
    }

    public virtual void Jump(T PlayerController)
    {

    }

    public virtual void SuperJump(T PlayerController)
    {

    }

    public virtual void GetDamage(T PlayerController)
    {

    }
}

[System.Serializable]
public class White : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("CharacterChange")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("CharacterChange");

        Managers.Instance.UiManager.PlayerSelectUIUpdate(0);

        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("White Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        changeEffect_obj.SetActive(true);
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        PlayerController.IsAttack = true;
        curDamage = normalDamage;

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;

        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {
        
    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("Dash")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;
    }

    public override void Jump(PlayerController PlayerController)
    {
        if (!PlayerController.IsJump || animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) return;

        if (PlayerController.IsDash) PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) animator.SetTrigger("JumpReady");
        animator.SetBool("isJump", true);

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Minimum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.rigid.AddForce(Vector3.up * PlayerController.jumpForce, ForceMode.Impulse);
        PlayerController.Invoke("JumpAir", 0.1f);
    }

    public override void SuperJump(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("SuperJump")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("SuperJump");

        PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GroundDown") && !PlayerController.IsGround)
        {
            PlayerController.IsSuperJump = true;
            animator.SetBool("GroundReady", false);
            animator.SetTrigger("GroundDown");
            Debug.LogError("qq");
        }
        else
        {
            PlayerController.IsSuperJump = false;
            animator.SetTrigger("GroundReadyAction");
        }

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;

        PlayerController.rigid.velocity = Vector3.zero;
        PlayerController.rigid.AddForce(Vector3.down * PlayerController.superJumpForce, ForceMode.Impulse);
    }

    public override void GetDamage(PlayerController PlayerController)
    {
        animator.SetTrigger("GetDamage");
    }
}

[System.Serializable]
public class Red : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("CharacterChange")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("CharacterChange");

        Managers.Instance.UiManager.PlayerSelectUIUpdate(1);

        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("Red Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        changeEffect_obj.SetActive(true);
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        PlayerController.IsAttack = true;
        curDamage = normalDamage;

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;

        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {

    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("Dash")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;
    }

    public override void Jump(PlayerController PlayerController)
    {
        if (!PlayerController.IsJump || animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) return;

        if (PlayerController.IsDash) PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) animator.SetTrigger("JumpReady");
        animator.SetBool("isJump", true);

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Minimum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.rigid.AddForce(Vector3.up * PlayerController.jumpForce, ForceMode.Impulse);
        PlayerController.Invoke("JumpAir", 0.1f);
    }

    public override void SuperJump(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("SuperJump")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("SuperJump");

        PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GroundDown") && !PlayerController.IsGround)
        {
            PlayerController.IsSuperJump = true;
            animator.SetBool("GroundReady", false);
            animator.SetTrigger("GroundDown");
            Debug.LogError("qq");
        }
        else
        {
            PlayerController.IsSuperJump = false;
            animator.SetTrigger("GroundReadyAction");
        }

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;

        PlayerController.rigid.velocity = Vector3.zero;
        PlayerController.rigid.AddForce(Vector3.down * PlayerController.superJumpForce, ForceMode.Impulse);
    }

    public override void GetDamage(PlayerController PlayerController)
    {
        animator.SetTrigger("GetDamage");
    }
}

[System.Serializable]
public class Green : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("CharacterChange")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("CharacterChange");

        Managers.Instance.UiManager.PlayerSelectUIUpdate(2);

        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("Green Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        changeEffect_obj.SetActive(true);
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        PlayerController.IsAttack = true;
        curDamage = normalDamage;

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;

        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {

    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("Dash")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;
    }

    public override void Jump(PlayerController PlayerController)
    {
        if (!PlayerController.IsJump || animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) return;

        if (PlayerController.IsDash) PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) animator.SetTrigger("JumpReady");
        animator.SetBool("isJump", true);

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Minimum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.rigid.AddForce(Vector3.up * PlayerController.jumpForce, ForceMode.Impulse);
        PlayerController.Invoke("JumpAir", 0.1f);
    }

    public override void SuperJump(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("SuperJump")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("SuperJump");

        PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GroundDown") && !PlayerController.IsGround)
        {
            PlayerController.IsSuperJump = true;
            animator.SetBool("GroundReady", false);
            animator.SetTrigger("GroundDown");
            Debug.LogError("qq");
        }
        else
        {
            PlayerController.IsSuperJump = false;
            animator.SetTrigger("GroundReadyAction");
        }

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;

        PlayerController.rigid.velocity = Vector3.zero;
        PlayerController.rigid.AddForce(Vector3.down * PlayerController.superJumpForce, ForceMode.Impulse);
    }

    public override void GetDamage(PlayerController PlayerController)
    {
        animator.SetTrigger("GetDamage");
    }
}

[System.Serializable]
public class Blue : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("CharacterChange")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("CharacterChange");

        Managers.Instance.UiManager.PlayerSelectUIUpdate(3);

        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("Blue Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        changeEffect_obj.SetActive(true);
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        PlayerController.IsAttack = true;
        curDamage = normalDamage;

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;

        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {

    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("Dash")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);

        Vector3 dirY = new Vector3(PlayerController.ray.direction.x, 0, PlayerController.ray.direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(dirY, Vector3.up);
        PlayerController.transform.rotation = targetRotation;
    }

    public override void Jump(PlayerController PlayerController)
    {
        if (!PlayerController.IsJump || animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) return;

        if (PlayerController.IsDash) PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpReady")) animator.SetTrigger("JumpReady");
        animator.SetBool("isJump", true);

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Minimum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.rigid.AddForce(Vector3.up * PlayerController.jumpForce, ForceMode.Impulse);
        PlayerController.Invoke("JumpAir", 0.1f);
    }

    public override void SuperJump(PlayerController PlayerController)
    {
        if (!Managers.Instance.CoolTimeManager.CoolCheck("SuperJump")) return;
        Managers.Instance.CoolTimeManager.GetCoolTime("SuperJump");

        PlayerController.IsDash = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GroundDown") && !PlayerController.IsGround)
        {
            PlayerController.IsSuperJump = true;
            animator.SetBool("GroundReady", false);
            animator.SetTrigger("GroundDown");
            Debug.LogError("qq");
        }
        else
        {
            PlayerController.IsSuperJump = false;
            animator.SetTrigger("GroundReadyAction");
        }

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;

        PlayerController.rigid.velocity = Vector3.zero;
        PlayerController.rigid.AddForce(Vector3.down * PlayerController.superJumpForce, ForceMode.Impulse);
    }

    public override void GetDamage(PlayerController PlayerController)
    {
        animator.SetTrigger("GetDamage");
    }
}