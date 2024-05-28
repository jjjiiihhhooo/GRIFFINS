using System.Collections;
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
    public float hitDelay;
    public float knockback;

    protected Transform target;

    public float modelShakeTime;

    public bool backHpHit;

    public bool isAction;

    public Vector3 knockbackDir;

    public bool isBossStart;

    public virtual void Init(EnemyController controller)
    {
        Debug.Log("ddddd");
        if (Player.Instance != null) target = Player.Instance.transform;
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
        if (_event != null) _event.Invoke();
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

    public virtual void UiUpdate()
    {

    }

    public virtual void ModelShake()
    {

    }

    public virtual void BossStart()
    {

    }

    public virtual Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight) //목표 위치까지 포물선 trajectoryHeight 높이 추가
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
          + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return (velocityXZ + velocityY);
    }
}

[System.Serializable]
public class Normal_Enemy : Enemy
{

    public float coolTime = 2f;
    public float curTime = 2f;

    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        Action();
        AttackDelay();
        ModelShake();
        UiUpdate();
    }

    public override void UiUpdate()
    {
        if (enemyController.uiShowDelay < 0f)
            enemyController.canvas.gameObject.SetActive(false);
        else
            enemyController.uiShowDelay -= Time.deltaTime;

        enemyController.hpSlider.value = Mathf.Lerp(enemyController.hpSlider.value, curHp / maxHp, Time.deltaTime * 5f);

        if (backHpHit)
        {
            enemyController.backHpSlider.value = Mathf.Lerp(enemyController.backHpSlider.value, enemyController.hpSlider.value, Time.deltaTime * 6f);
            if (enemyController.hpSlider.value >= enemyController.backHpSlider.value - 0.001f)
            {
                backHpHit = false;
                enemyController.backHpSlider.value = enemyController.hpSlider.value;
            }
        }

        enemyController.canvas.transform.LookAt(enemyController.canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private void AttackDelay()
    {
        if (curTime > 0) curTime -= Time.deltaTime;
    }


    public override void ModelShake()
    {
        if (modelShakeTime > 0f)
        {
            modelShakeTime -= Time.deltaTime;
            animator.transform.localPosition = Random.insideUnitSphere * 0.3f + Vector3.zero;
            if (modelShakeTime <= 0f) animator.transform.localPosition = Vector3.zero;
        }
    }

    public override void Action()
    {
        if (target == null) target = Player.Instance.transform;

        if (enemyController.isHit)
        {
            enemyController.hitCool -= Time.deltaTime;
            Vector3 playerPos = new Vector3(target.transform.position.x, enemyController.transform.position.y, target.transform.position.z);
            Vector3 KnockbackDir;

            if (knockbackDir == Vector3.zero)
                KnockbackDir = -enemyController.transform.forward;
            else
            {
                KnockbackDir = knockbackDir;
            }
            enemyController.rigid.velocity = Vector3.zero;
            enemyController.rigid.AddForce(KnockbackDir * knockback, ForceMode.VelocityChange);

            if (enemyController.hitCool < 0)
            {
                enemyController.rigid.velocity = Vector3.zero;
                enemyController.isHit = false;
            }
            return;
        }

        if (Vector3.Distance(target.transform.position, enemyController.transform.position) > 3f && curTime <= 0 && !isAction) Move();
        else if (Vector3.Distance(target.transform.position, enemyController.transform.position) <= 3f && curTime <= 0 && !isAction) Attack();

    }

    private void NormalAttack()
    {
        Debug.Log("NormalAttack");
        isAction = true;
        animator.Play("Attack_Boss", 0, 0f);
        curTime = coolTime;
    }

    private void Attack()
    {
        Vector3 forward = target.transform.position - enemyController.transform.position;
        float x = enemyController.transform.eulerAngles.x;
        float z = enemyController.transform.eulerAngles.z;

        enemyController.transform.forward = forward;
        enemyController.transform.eulerAngles = new Vector3(x, enemyController.transform.eulerAngles.y, z);
        NormalAttack();
    }

    private void Move()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_Boss")) animator.Play("Walking_Boss", 0, 0f);


        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        //enemyController.rigid.AddForce(enemyController.transform.forward.normalized * moveSpeed , ForceMode.VelocityChange);

        //enemyController.rigid.velocity = enemyController.transform.forward * moveSpeed;
        enemyController.transform.position = enemyController.transform.position + enemyController.transform.forward.normalized * moveSpeed * Time.deltaTime;
    }

    public override void GetDamage(float damage)
    {

        enemyController.uiShowDelay = 4f;
        enemyController.canvas.gameObject.SetActive(true);

        //if (animator != null) animator.Play("GetDamage", 0, 0);
        curHp -= damage;
        backHpHit = false;
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["enemyHit"], false);
        enemyController.Invoke("BackHpFunMessage", 0.3f);

        if (curHp <= 0) Die();
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
public class Epic_1_Enemy : Epic_Enemy
{
    //public float coolTime = 3f;
    //public float curTime = 3f;

    //public Transform target;

    //public override void Init(EnemyController controller)
    //{
    //    base.Init(controller);
    //}

    //public override void EnemyUpdate()
    //{
    //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Boss")) animator.Play("Idle_Boss", 0, 0f);

    //    AttackDelay();
    //}

    //private void AttackDelay()
    //{
    //    if (curTime > 0) curTime -= Time.deltaTime;
    //    else Action();
    //}

    //public override void Action()
    //{
    //    if (target == null) target = Player.Instance.transform;

    //    Shoot();
    //}

    //private void Shoot()
    //{
    //    curTime = coolTime;
    //    enemyController.transform.LookAt(target.transform);
    //    enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);
    //    GameObject temp = GameObject.Instantiate(obj, fireTransform.position, Quaternion.identity);
    //    temp.GetComponent<EnemyBullet>().dir = target.position - temp.transform.position;
    //    temp.SetActive(true);
    //}

    //public override void Die()
    //{
    //    enemyController.DeadMessage();
    //}

    //public override void GetDamage(float damage)
    //{
    //    if (curHp <= 0) Die();

    //    curHp -= damage;
    //    backHpHit = false;
    //    enemyController.Invoke("BackHpFunMessage", 0.3f);
    //}

}

[System.Serializable]
public class Boss_Destroyer : Enemy
{
    [SerializeField] private int trackingCount;
    [SerializeField] private int bombingCount;
    [SerializeField] private int bombingBulletCount;
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject bombingArea;
    [SerializeField] private GameObject pushingArea;
    [SerializeField] private GameObject[] fireWaveArea;
    [SerializeField] private GameObject[] pizzaArea;
    [SerializeField] private GameObject trackingFireEffect;
    [SerializeField] private GameObject swingArea;
    [SerializeField] private Transform firePos;

    private float attackCurDelay;

    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        //UiUpdate();
        if (target == null) target = Player.Instance.transform;
        RandomPattern();

        if (enemyController.isHit)
        {
            enemyController.hitCool -= Time.deltaTime;
            if (enemyController.hitCool < 0)
            {
                enemyController.isHit = false;
            }
        }

        //TrackingBullet();
        //Bombing();
    }

    public override void Action()
    {
        base.Action();
    }

    public override void UiUpdate()
    {
        enemyController.hpSlider.value = Mathf.Lerp(enemyController.hpSlider.value, curHp / maxHp, Time.deltaTime * 5f);

        if (backHpHit)
        {
            enemyController.backHpSlider.value = Mathf.Lerp(enemyController.backHpSlider.value, enemyController.hpSlider.value, Time.deltaTime * 6f);
            if (enemyController.hpSlider.value >= enemyController.backHpSlider.value - 0.001f)
            {
                backHpHit = false;
                enemyController.backHpSlider.value = enemyController.hpSlider.value;
            }
        }
    }

    private void RandomPattern()
    {
        if (attackCurDelay > 0)
        {
            attackCurDelay -= Time.deltaTime;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BossIdle")) animator.Play("BossIdle", 0, 0f);
            return;
        }

        int rand = Random.Range(0, 101);

        if (rand <= 25)
        {
            TrackingBullet();
        }
        else if (25 < rand && rand <= 40)
        {
            Bombing();
        }
        else if (40 < rand && rand <= 55)
        {
            FireWave();
        }
        else if (55 < rand && rand <= 70)
        {
            ContinuousPizza();
        }
        else if (70 < rand && rand <= 85)
        {
            Pushing();
        }
        else if (85 < rand && rand <= 100)
        {
            RightSwing();
        }
        

    }

    private void RightSwing()
    {
        if (isAction) return;
        isAction = true;
        target.GetComponent<Player>().CoroutineEvent(SwingCor());
    }

    private IEnumerator SwingCor()
    {
        
        //animator.Play("FireWave", 0, 0f);
        enemyController.transform.forward = target.transform.position - enemyController.transform.position;
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);


        GameObject.Instantiate(swingArea, enemyController.transform.position, enemyController.transform.rotation);

        yield return new WaitForSeconds(0.5f);
        
        if (animator != null)
            animator.Play("Boss_Idle", 0, 0f);

        isAction = false;
        attackCurDelay = attackDelay;

    }

    private void Pushing()
    {
        if (isAction) return;
        isAction = true;
        target.GetComponent<Player>().CoroutineEvent(PushingCor());
    }

    private IEnumerator PushingCor()
    {
        GameObject temp = GameObject.Instantiate(pushingArea, enemyController.transform.position, Quaternion.identity);
        SphereCollider col = temp.GetComponent<SphereCollider>();

        if (Vector3.Distance(target.position, enemyController.transform.position) < 5f) target.GetComponent<Player>().GetDamage(3);

        while (col.radius < 20)
        {
            yield return new WaitForEndOfFrame();
            col.radius += 0.3f;
        }

        col.radius = 0;
        GameObject.Destroy(col.gameObject);

        if (animator != null)
            animator.Play("Boss_Idle", 0, 0f);

        isAction = false;
        attackCurDelay = attackDelay;
    }

    private void ContinuousPizza()
    {
        if (isAction) return;
        isAction = true;
        target.GetComponent<Player>().CoroutineEvent(ContinuousPizzaCor());
    }

    private IEnumerator ContinuousPizzaCor()
    {
        for (int i = 0; i < pizzaArea.Length; i++)
        {
            if (animator == null) break;

            GameObject obj = GameObject.Instantiate(pizzaArea[i], enemyController.transform.position, Quaternion.identity);
            //obj.transform.forward = enemyController.transform.forward;
            yield return new WaitForSeconds(1f);
        }

        if (animator != null)
            animator.Play("Boss_Idle", 0, 0f);

        isAction = false;
        attackCurDelay = attackDelay;
    }

    private void FireWave()
    {
        if (isAction) return;
        isAction = true;
        target.GetComponent<Player>().CoroutineEvent(FireWaveCor());
    }

    private IEnumerator FireWaveCor()
    {
        animator.Play("FireWave", 0, 0f);
        enemyController.transform.forward = target.transform.position - enemyController.transform.position;
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);


        for (int i = 0; i < fireWaveArea.Length; i++)
        {
            if (animator == null) break;

            GameObject obj = GameObject.Instantiate(fireWaveArea[i], enemyController.transform.position, Quaternion.identity);
            //obj.transform.forward = enemyController.transform.forward;
            yield return new WaitForSeconds(0.8f);
        }
        if (animator != null)
            animator.Play("Boss_Idle", 0, 0f);
        isAction = false;
        attackCurDelay = attackDelay;
    }

    private void TrackingBullet()
    {
        if (isAction) return;
        isAction = true;
        target.GetComponent<Player>().CoroutineEvent(TrackingCor());
    }

    private IEnumerator TrackingCor()
    {
        float time = 2f;
        Vector3 endPos;
        Vector3 dir;

        animator.Play("Tracking", 0, 0f);

        while (time > 0)
        {
            if (target != null)
            {
                if (animator == null) break;
                endPos = target.position;
                dir = endPos - firePos.position;
                enemyController.transform.forward = target.transform.position - enemyController.transform.position;
                enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);
                GameObject projectile = GameObject.Instantiate(trackingFireEffect, firePos.position, Quaternion.identity);
                projectile.transform.forward = dir;
                projectile.gameObject.SetActive(true);
            }

            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        attackCurDelay = attackDelay;
        isAction = false;


        if (animator != null)
            animator.Play("Boss_Idle", 0, 0f);
    }

    private void Bombing()
    {
        if (isAction) return;
        isAction = true;

        target.GetComponent<Player>().CoroutineEvent(BombingCor());

    }

    private IEnumerator BombingCor()
    {
        Vector3[] positions = new Vector3[bombingBulletCount];
        GameObject[] gameObjects = new GameObject[bombingBulletCount];

        animator.Play("Bombing", 0, 0f);

        for (int i = 0; i < bombingCount; i++)
        {
            if (animator == null) break;
            yield return new WaitForSeconds(0.5f);
            for (int j = 0; j < bombingBulletCount; j++)
            {
                if (animator == null) break;
                if (enemyController != null)
                {
                    enemyController.transform.forward = target.transform.position - enemyController.transform.position;
                    enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);
                    Vector3 pos = new Vector3(enemyController.transform.position.x + Random.Range(-15f, 15f), 0f, enemyController.transform.position.z + Random.Range(-15f, 15f));
                    positions[j] = pos;
                    gameObjects[j] = GameObject.Instantiate(bombingArea, pos, Quaternion.identity);
                    yield return new WaitForSeconds(0.2f);
                }
            }

            //for(int k = 0; k < bombingBulletCount; k++)
            //{
            //    GameObject.Destroy(gameObjects[k].gameObject);
            //    yield return new WaitForSeconds(0.2f);
            //    GameObject.Instantiate(bombingEffect, positions[k], Quaternion.identity);
            //}

        }

        if (animator != null)
            animator.Play("Boss_Idle", 0, 0f);
        isAction = false;
        attackCurDelay = attackDelay;
    }



    public override void GetDamage(float damage)
    {
        curHp -= damage;
        backHpHit = false;
        enemyController.anim_dot.DORestartById("Hit");
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["enemyHit"], false);
        enemyController.Invoke("BackHpFunMessage", 0.3f);

        if (curHp <= 0)
        {
            target.GetComponent<Player>().CoroutineExit(BombingCor());
            target.GetComponent<Player>().CoroutineExit(TrackingCor());
            Die();
        }
    }

    public override void Die()
    {
        enemyController.DeadMessage();
    }
}