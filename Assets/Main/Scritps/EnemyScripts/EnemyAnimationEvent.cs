
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private EnemyController _enemy;

    private void Awake()
    {
        _enemy = transform.parent.GetComponent<EnemyController>();
    }

    public void NormalAttack()
    {
        _enemy.NormalAttackAnim();
    }

    public void BombingExit()
    {
        _enemy.enemy.BombingAnimExit();
    }

    public void TrackingExit()
    {
        _enemy.enemy.TrackingAnimExit();
    }

    public void AttackStart()
    {
        _enemy.StartAttackAnim();
    }

    public void Normal_Enemy_Attack()
    {
        _enemy.attackCol.gameObject.SetActive(true);
    }



    public void Normal_Enemy_RunAttack()
    {
        _enemy.runAttackCol.gameObject.SetActive(true);
        _enemy.enemy.isRun = false;
    }


    public void Epic_Enemy_NormalAttack()
    {
        _enemy.enemy.attackCurCool = _enemy.enemy.attackMaxCool;
        _enemy.attackCol.gameObject.SetActive(true);
        //Instantiate(_enemy.enemy.attackEffect, transform.position, Quaternion.identity);
    }

    public void Epic_Enemy_HammerAttack()
    {
        _enemy.enemy.attackCurCool = _enemy.enemy.attackMaxCool;
        _enemy.enemy.hammerCurTime = _enemy.enemy.hammerCoolTime;
        _enemy.hammerCol.gameObject.SetActive(true);
        Instantiate(_enemy.enemy.attackEffect, transform.position, Quaternion.identity);
    }


    public void AttackExit()
    {
        _enemy.enemy.attackCurCool = _enemy.enemy.attackMaxCool;
    }

    public void Dead()
    {
        _enemy.DeadMessage();
    }
}
