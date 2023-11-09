using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy
{
    public ParticleSystem damageEffect;
    public Animator animator;
    public EnemyController enemyController;
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    public float attackDamage;
    public float attackDelay;
    public float hitDelay;

    public virtual void Init(EnemyController controller)
    {
        Debug.Log("ddddd");
        enemyController = controller;
        curHp = maxHp;
    }

    public virtual void GetDamage(float damage)
    {
        if (animator != null) animator.SetTrigger("GetDamage");
        curHp -= damage;

        if (curHp <= 0) Die();
        Debug.Log(curHp);
    }

    public virtual void Action()
    {

    }

    public virtual void Die()
    {
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

    public override void Action()
    {
        base.Action();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }
}
