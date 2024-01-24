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

    public virtual void Init(EnemyController controller)
    {
        Debug.Log("ddddd");
        enemyController = controller;
        curHp = maxHp;
    }

    public virtual void GetDamage(float damage)
    {
        if (curHp <= 0) Die();

        if (animator != null) animator.SetTrigger("GetDamage");
        curHp -= damage;

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
        if (animator == null) enemyController.Dead();
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
        base.GetDamage(damage);
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
