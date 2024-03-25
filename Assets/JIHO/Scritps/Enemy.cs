using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;


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

    public bool isAction;

    public bool isBossStart;

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
        if (animator == null)
        {
            enemyController.DeadMessage();
            return;
        }
        if (animator.GetBool("Dead")) return;
        animator.SetBool("Dead", true);

        animator.SetTrigger("Die");
    }

    public virtual void BossStart()
    {

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


    private bool isDefense;
    private bool isRangeAttack;
    private bool superArmor;
    public float coolTime = 2f;
    public float curTime = 2f;

    public GameObject[] defenseObjects;
    public Transform[] defenseTransforms;

    public GameObject rangeAttack;

    public Transform startPos;
    public Transform rangeTransform;
    private Transform target;

    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        if (isBossStart)
        {
            Action();
            AttackDelay();
        }
    }

    public override void BossStart()
    {
        if (isBossStart) return;
        if (enemyController.anim_dot == null) enemyController.anim_dot = GameManager.Instance.uiManager.bossHpDot;
        isBossStart = true;
        isDefense = false;
        isRangeAttack = false;
        curHp = maxHp;
        enemyController.transform.position = startPos.position;
        curTime = 0;
        enemyController.anim_dot.DOPlayById("Start");
    }

    private void AttackDelay()
    {
        if (curTime > 0) curTime -= Time.deltaTime;
    }

    private void Move()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_Boss")) animator.Play("Walking_Boss", 0, 0f);


        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        enemyController.transform.position = enemyController.transform.position + enemyController.transform.forward.normalized * moveSpeed * Time.deltaTime;
    }

    private void NormalAttack()
    {
        Debug.Log("NormalAttack");
        isAction = true;
        animator.Play("Attack_Boss", 0, 0f);
        curTime = coolTime;
    }

    private void DefenseMode()
    {
        isAction = true;
        enemyController.CorEvent(DefenseCor());
    }

    public IEnumerator DefenseCor()
    {
        float time = 20f;
        enemyController.rigid.useGravity = false;
        superArmor = true;
        Vector3 data = new Vector3(0, 1f, 0);
        animator.Play("Defense_Boss", 0, 0f);

        while(enemyController.transform.position.y < 50)
        {
            yield return new WaitForEndOfFrame();
            enemyController.transform.position += data;
        }

        enemyController.transform.position = rangeTransform.position;


        while (enemyController.transform.position.y > 5)
        {
            yield return new WaitForEndOfFrame();
            enemyController.transform.position -= data;
        }

        enemyController.rigid.useGravity = true;

        float y = Player.Instance.transform.position.y + 8f;

        for(int i = 0; i < 10; i++)
        {
            float x = Random.Range(-5f, 5f);
            if (x > 0) x += 5f;
            else x -= 5f;

            float z = Random.Range(-5f, 5f);
            if (z > 0) z += 5f;
            else z -= 5f;

            GameObject temp = GameObject.Instantiate(obj, new Vector3(enemyController.transform.position.x + x, y, enemyController.transform.position.z + z), Quaternion.identity);
            temp.SetActive(true);
        }


        yield return new WaitForSeconds(1f);

        for(int i = 0; i < defenseObjects.Length; i++)
        {
            defenseObjects[i].SetActive(true);
            defenseObjects[i].transform.position = defenseTransforms[i].position;
        }

        while (time > 0)
        {
            bool test = false;
            time -= Time.deltaTime;
            GameManager.Instance.uiManager.bossTiming.value = time / 10;
            curHp += Time.deltaTime * 2f;
            enemyController.backHpSlider.value = enemyController.hpSlider.value;
            for(int i = 0; i < defenseObjects.Length; i++)
            {
                if (defenseObjects[i].activeSelf) test = true;
            }

            if (!test) time = 0;
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < defenseObjects.Length; i++)
        {
            defenseObjects[i].SetActive(false);
        }

        superArmor = false;
        GameManager.Instance.uiManager.bossTiming.value = 0;
        isAction = false;
        isDefense = true;
    }


    private void RangeAttack()
    {
        isAction = true;
        enemyController.CorEvent(RangeAttackCor());
    }

    public IEnumerator RangeAttackCor()
    {
        float time = 5f;
        enemyController.rigid.useGravity = false;
        superArmor = true;

        Vector3 data = new Vector3(0, 1f, 0);
        animator.Play("Defense_Boss", 0, 0f);

        while (enemyController.transform.position.y < 50)
        {
            yield return new WaitForEndOfFrame();
            enemyController.transform.position += data;
        }

        enemyController.transform.position = rangeTransform.position;


        while (enemyController.transform.position.y > 5)
        {
            yield return new WaitForEndOfFrame();
            enemyController.transform.position -= data;
        }

        enemyController.rigid.useGravity = true;

        while (time > 0)
        {
            time -= Time.deltaTime;
            GameManager.Instance.uiManager.bossTiming.value = time / 5;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.uiManager.bossTiming.value = 0;
        superArmor = false;
        isAction = false;
        isRangeAttack = true;
        rangeAttack.SetActive(true);
    }

    private void Attack()
    {
        Vector3 forward = target.transform.position - enemyController.transform.position;
        float x = enemyController.transform.eulerAngles.x;
        float z = enemyController.transform.eulerAngles.z;

        enemyController.transform.forward = forward;
        enemyController.transform.eulerAngles = new Vector3(x, enemyController.transform.eulerAngles.y, z);

        if (!isRangeAttack && curHp < 60f) RangeAttack();
        else if (!isDefense && curHp < 30f) DefenseMode();
        else NormalAttack();
    }

    public override void Action()
    {
        if (target == null) target = Player.Instance.transform;

        if (Vector3.Distance(target.transform.position, enemyController.transform.position) > 10f && curTime <= 0 && !isAction) Move();
        else if (Vector3.Distance(target.transform.position, enemyController.transform.position) <= 10f && curTime <= 0 && !isAction) Attack();
    }

    public override void Die()
    {
        enemyController.DeadMessage();
    }

    public override void GetDamage(float damage)
    {
        if (curHp <= 0) Die();

        if(!superArmor)
            curHp -= damage;
        backHpHit = false;

        enemyController.Invoke("BackHpFunMessage", 0.3f);

        Debug.Log(curHp);
        enemyController.anim_dot.DORestartById("GetDamage");
    }
}
