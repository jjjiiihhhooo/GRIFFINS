using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public class Unit<T>
{

    public State<T> currentState;
    public Animator animator;
    public GameObject unit_obj;
    public GameObject attackCol;

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
}

[System.Serializable]
public class White : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("White Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        animator.SetTrigger("ChangeUnit");
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        //animator.SetLayerWeight(0, 0f);
        //animator.SetLayerWeight(1, 1f);
        PlayerController.IsAttack = true;
        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {
        
    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!CoolTimeManager.Instance.CoolCheck("Dash")) return;
        CoolTimeManager.Instance.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);
        PlayerController.transform.forward = PlayerController.ray.direction;
        PlayerController.transform.rotation = Quaternion.Euler(new Vector3(0, PlayerController.transform.rotation.y, 0));
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
        if (!CoolTimeManager.Instance.CoolCheck("SuperJump")) return;
        CoolTimeManager.Instance.GetCoolTime("SuperJump");

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
}

[System.Serializable]
public class Red : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("Red Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        animator.SetTrigger("ChangeUnit");
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        //animator.SetLayerWeight(0, 0f);
        //animator.SetLayerWeight(1, 1f);
        PlayerController.IsAttack = true;
        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {

    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!CoolTimeManager.Instance.CoolCheck("Dash")) return;
        CoolTimeManager.Instance.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);
        PlayerController.transform.forward = PlayerController.ray.direction;
        PlayerController.transform.rotation = Quaternion.Euler(new Vector3(0, PlayerController.transform.rotation.y, 0));
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
        if (!CoolTimeManager.Instance.CoolCheck("SuperJump")) return;
        CoolTimeManager.Instance.GetCoolTime("SuperJump");

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
}

[System.Serializable]
public class Green : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("Green Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        animator.SetTrigger("ChangeUnit");
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        //animator.SetLayerWeight(0, 0f);
        //animator.SetLayerWeight(1, 1f);
        PlayerController.IsAttack = true;
        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {

    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!CoolTimeManager.Instance.CoolCheck("Dash")) return;
        CoolTimeManager.Instance.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);
        PlayerController.transform.forward = PlayerController.ray.direction;
        PlayerController.transform.rotation = Quaternion.Euler(new Vector3(0, PlayerController.transform.rotation.y, 0));
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
        if (!CoolTimeManager.Instance.CoolCheck("SuperJump")) return;
        CoolTimeManager.Instance.GetCoolTime("SuperJump");

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
}

[System.Serializable]
public class Blue : Unit<PlayerController>
{
    public override void ChangeUnit(PlayerController PlayerController)
    {
        PlayerController.currentUnit.Exit(PlayerController);
        PlayerController.currentUnit = this;
        Debug.Log("Blue Change");
        Enter(PlayerController);
    }

    public override void Enter(PlayerController PlayerController)
    {
        unit_obj.SetActive(true);
        animator.SetTrigger("ChangeUnit");
    }

    public override void Exit(PlayerController PlayerController)
    {
        unit_obj.SetActive(false);
    }

    public override void AttackAction(PlayerController PlayerController)
    {
        //animator.SetLayerWeight(0, 0f);
        //animator.SetLayerWeight(1, 1f);
        PlayerController.IsAttack = true;
        animator.SetTrigger("AttackCombo");
    }

    public override void SkillAction(PlayerController PlayerController)
    {

    }

    public override void Dash(PlayerController PlayerController)
    {
        if (!CoolTimeManager.Instance.CoolCheck("Dash")) return;
        CoolTimeManager.Instance.GetCoolTime("Dash");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.SetTrigger("Dash");

        PlayerController.pm.bounceCombine = PhysicMaterialCombine.Maximum;
        PlayerController.groundTime = PlayerController.groundMaxTime;
        PlayerController.IsDash = true;
        animator.SetBool("isDashAir", true);

        Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        PlayerController.ray = new Ray(PlayerController.transform.position, dir);

        PlayerController.rigid.AddForce(PlayerController.ray.direction * PlayerController.DashSpeed, ForceMode.Impulse);
        PlayerController.transform.forward = PlayerController.ray.direction;
        PlayerController.transform.rotation = Quaternion.Euler(new Vector3(0, PlayerController.transform.rotation.y, 0));
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
        if (!CoolTimeManager.Instance.CoolCheck("SuperJump")) return;
        CoolTimeManager.Instance.GetCoolTime("SuperJump");

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
}