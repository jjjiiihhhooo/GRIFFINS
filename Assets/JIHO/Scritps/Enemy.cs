using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy
{
    public GameObject damageEffect;
    public Animator animator;
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    public float attackDamage;
    public float attackDelay;
    //dd

    public virtual void GetDamage(float damage)
    {

    }

    public virtual void Action()
    {

    }

}

[System.Serializable]
public class Normal_Enemy : Enemy
{
    public override void Action()
    {
        
    }

    public override void GetDamage(float damage)
    {
        
    }
}

[System.Serializable]
public class Epic_Enemy : Enemy
{
    public override void Action()
    {

    }

    public override void GetDamage(float damage)
    {

    }
}

[System.Serializable]
public class Boss_Enemy : Enemy
{
    public override void Action()
    {

    }

    public override void GetDamage(float damage)
    {

    }
}
