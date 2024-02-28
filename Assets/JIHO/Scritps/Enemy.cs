using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Enemy
{
    public ParticleSystem damageEffect;
    public Animator animator;
    public EnemyController enemyController;
    public Transform fireTransform;
    public GameObject obj;
    public UnityEvent _event;
    public string name;

    public float maxHp;
    public float curHp;
    public float moveSpeed;
    public float attackDamage;
    public float attackCurCool;
    public float attackMaxCool;
    public float rangeX;
    public float rangeZ;

    public bool backHpHit;

    public virtual void Init(EnemyController controller)
    {
        Debug.Log("ddddd");
        enemyController = controller;
        animator = controller.animator;
        curHp = maxHp;
    }

    public virtual void GetDamage(float damage)
    {

        
        if (animator != null) animator.SetTrigger("GetDamage");
        curHp -= damage;
        if (curHp <= 0) Die();
        enemyController.Invoke("BackHpMessage", 0.5f);
        Debug.Log(curHp);
    }

    public virtual void EnemyUpdate()
    {

    }

    public virtual void Action()
    {
        if(_event != null) _event.Invoke();
    }

    public virtual void Die()
    {
        if (animator == null) enemyController.DeadMessage();
        if (animator.GetBool("Dead")) return;
        animator.SetBool("Dead", true);

        animator.SetTrigger("Die");
    }

}

[System.Serializable]
public class Normal_Enemy : Enemy
{
    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        if(attackCurCool > 0)
        {
            attackCurCool -= Time.deltaTime;
        }
        else
        {
            attackCurCool = attackMaxCool;
            Action();
        }
    }

    public override void Action()
    {
        base.Action();
    }

    public override void GetDamage(float damage)
    {
        if (curHp <= 0) Die();

        Vector3 playerPos = new Vector3(Player.Instance.transform.position.x, enemyController.transform.position.y, Player.Instance.transform.position.z);

        Vector3 KnockbackDir = enemyController.transform.position - playerPos;


        enemyController.rigid.AddForce(KnockbackDir * 2f, ForceMode.Impulse);

        if (animator != null) animator.Play("GetDamage", 0, 0);
        curHp -= damage;
        backHpHit = false;
        enemyController.Invoke("BackHpFunMessage", 0.3f);
        Debug.Log(curHp);
    }

    public override void Die()
    {
        //animator.Play("Dead");
        enemyController.DeadMessage();
    }
}

[System.Serializable]
public class Epic_Enemy : Enemy
{
    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        base.EnemyUpdate();
    }

    public override void Action()
    {
        base.Action();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }
}

[System.Serializable]
public class Boss_Enemy : Enemy
{
    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        base.EnemyUpdate();
    }

    public override void Action()
    {
        base.Action();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }
}
