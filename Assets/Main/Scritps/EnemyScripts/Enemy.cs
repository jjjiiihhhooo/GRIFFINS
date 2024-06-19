using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Enemy
{
    public ParticleSystem damageEffect;
    public GameObject attackEffect;
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

    public float hammerCoolTime = 11f;
    public float hammerCurTime = 0f;


    public Transform target;

    public float modelShakeTime;

    public bool backHpHit;

    public bool isAction;
    public bool isHammer = true;
    public bool isRun;

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

    public virtual Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
          + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return (velocityXZ + velocityY);
    }

    public virtual void BombingAnimExit()
    {

    }

    public virtual void TrackingAnimExit()
    {

    }
}

[System.Serializable]
public class Normal_Enemy : Enemy
{
    public float coolTime = 3f;
    public float curTime = 3f;

    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        if (enemyController.isDead) return;

        if (attackCurCool > 0 && isAction)
        {
            attackCurCool -= Time.deltaTime;
            if (attackCurCool < 0) isAction = false;
        }

        if (enemyController.isHit)
        {
            enemyController.hitCool -= Time.deltaTime;

            if (enemyController.hitCool < 0)
            {
                enemyController.rigid.velocity = Vector3.zero;
                enemyController.isHit = false;
            }

            return;
        }



        Action();
        AnimTransform();
        //ModelShake();
        UiUpdate();
    }

    public void AnimTransform()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //Debug.LogError(model.transform.localPosition);

            RaycastHit hit;

            if (Physics.Raycast(enemyController.transform.position + Vector3.up, enemyController.transform.forward, out hit, 2f, enemyController.animCheckLayer))
            {
                animator.transform.localPosition = new Vector3(0f, animator.transform.localPosition.y, 0f);
            }

            enemyController.transform.position = animator.transform.position;
            animator.transform.localPosition = Vector3.zero;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("GetDamage"))
        {

            RaycastHit hit;

            if (Physics.Raycast(enemyController.transform.position + Vector3.up, -enemyController.transform.forward, out hit, 2f, enemyController.animCheckLayer))
            {
                animator.transform.localPosition = new Vector3(0f, animator.transform.localPosition.y, 0f);
            }
            enemyController.transform.position = animator.transform.position;
            animator.transform.localPosition = Vector3.zero;
        }
        else
        {
            animator.transform.localPosition = Vector3.zero;
            float x = animator.transform.localEulerAngles.x;
            float y = animator.transform.localEulerAngles.y;
            float z = animator.transform.localEulerAngles.z;

            if (x != 0 || y != 0 || z != 0)
                animator.transform.localEulerAngles = Vector3.zero;
        }

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



        if (Vector3.Distance(target.transform.position, enemyController.transform.position) > 3f && curTime <= 0 && !isAction) Move();
        else if (Vector3.Distance(target.transform.position, enemyController.transform.position) <= 3f && curTime <= 0 && !isAction) Attack();

    }

    private void NormalAttack()
    {
        isAction = true;
        enemyController.rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        animator.Play("NormalAttack_1", 0, 0f);
        curTime = coolTime;
    }

    private void RunAttack()
    {
        isAction = true;
        enemyController.rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        animator.Play("Normal_RunAttack_1", 0, 0f);
        curTime = coolTime;
    }

    private void Attack()
    {
        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        enemyController.rigid.velocity = Vector3.zero;

        if (isRun) RunAttack();
        else NormalAttack();
    }

    private void Move()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Normal_Run")) animator.Play("Normal_Run", 0, 0f);

        if (!isRun)
        {
            isRun = true;
        }

        bool temp = (enemyController.rigid.constraints & RigidbodyConstraints.FreezePosition) != 0;

        if (temp) enemyController.rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;


        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        //enemyController.rigid.AddForce(enemyController.transform.forward.normalized , ForceMode.VelocityChange);

        enemyController.rigid.velocity = enemyController.transform.forward * moveSpeed;
        //enemyController.transform.position = enemyController.transform.position + enemyController.transform.forward.normalized * moveSpeed * Time.deltaTime;
    }

    public override void GetDamage(float damage)
    {
        if (enemyController.isDead) return;
        enemyController.uiShowDelay = 4f;
        enemyController.canvas.gameObject.SetActive(true);
        int temp = Random.Range(1, 3);
        string temp2 = "GetDamage_" + temp.ToString();
        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);
        //if(!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        animator.Play(temp2, 0, 0);
        attackCurCool = 1.0f;
        curHp -= damage;
        backHpHit = false;
        GameManager.Instance.soundManager.Play("enemyHit", false);
        enemyController.Invoke("BackHpFunMessage", 0.3f);

        if (curHp <= 0) Die();
    }



    public override void Die()
    {
        enemyController.isDead = true;

        enemyController.gameObject.layer = enemyController.deadLayer;
        animator.Play("Dead");
    }
}

[System.Serializable]
public class Epic_Enemy : Enemy
{
    public float coolTime = 5f;
    public float curTime = 5f;



    public float dashSpeed;


    public override void Init(EnemyController controller)
    {
        base.Init(controller);
    }

    public override void EnemyUpdate()
    {
        if (enemyController.isDead) return;

        if (attackCurCool > 0 && isAction)
        {
            attackCurCool -= Time.deltaTime;
            if (attackCurCool < 0) isAction = false;
        }

        if (hammerCurTime > 0 && !isHammer)
        {
            hammerCurTime -= Time.deltaTime;
            if (hammerCurTime < 0) isHammer = true;
        }

        if (enemyController.isHit)
        {
            enemyController.hitCool -= Time.deltaTime;

            if (enemyController.hitCool < 0)
            {
                //enemyController.rigid.velocity = Vector3.zero;
                enemyController.isHit = false;
            }

            return;
        }

        Action();
        //AnimTransform();
        //ModelShake();
        UiUpdate();
    }

    public void AnimTransform()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //Debug.LogError(model.transform.localPosition);

            RaycastHit hit;

            if (Physics.Raycast(enemyController.transform.position + Vector3.up, enemyController.transform.forward, out hit, 2f, enemyController.animCheckLayer))
            {
                animator.transform.localPosition = new Vector3(0f, animator.transform.localPosition.y, 0f);
            }

            enemyController.transform.position = animator.transform.position;
            animator.transform.localPosition = Vector3.zero;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("GetDamage"))
        {
            enemyController.transform.position = animator.transform.position;
            animator.transform.localPosition = Vector3.zero;
        }
        else
        {
            animator.transform.localPosition = Vector3.zero;
            float x = animator.transform.localEulerAngles.x;
            float y = animator.transform.localEulerAngles.y;
            float z = animator.transform.localEulerAngles.z;

            if (x != 0 || y != 0 || z != 0)
                animator.transform.localEulerAngles = Vector3.zero;
        }
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

        if (isHammer)
        {
            if (Vector3.Distance(target.transform.position, enemyController.transform.position) < 10f && !isAction) HammerDown(target.transform.position);
            else if (Vector3.Distance(target.transform.position, enemyController.transform.position) > 10f && !isAction) Dash();
        }
        else
        {
            if (Vector3.Distance(target.transform.position, enemyController.transform.position) <= 3f && !isAction) Attack();
            else if (Vector3.Distance(target.transform.position, enemyController.transform.position) > 3f && !isAction) Move();
        }
    }

    private void HammerDown(Vector3 endPos)
    {

        isAction = true;
        isHammer = false;
        Vector3 velocity = HammerVelocity(enemyController.transform.position, endPos, 1f);

        target.GetComponent<Player>().CoroutineEvent(HammerCor(enemyController.transform.position, target.transform.position));
    }

    private Vector3 HammerVelocity(Vector3 start, Vector3 end, float t)
    {
        float parabolaHeight = Mathf.Abs(start.y - end.y) + 4; // 포물선의 높이 조정, 필요시 수정
        float parabola = -4 * parabolaHeight * (t - 0.5f) * (t - 0.5f) + parabolaHeight;

        // 선형 보간을 사용하여 x, z 위치 계산
        Vector3 linearPosition = Vector3.Lerp(start, end, t);

        // y 위치에 포물선 값 추가
        return new Vector3(linearPosition.x, linearPosition.y + parabola, linearPosition.z);
    }

    private IEnumerator HammerCor(Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0;
        float flightDuration = 1f;

        if (!enemyController.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) animator.Play("HammerDown", 0, 0f);
        else enemyController.animator.SetBool("isHammer", true);

        while (elapsedTime < flightDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flightDuration;

            // 포물선 경로 계산
            Vector3 currentPosition = HammerVelocity(startPosition, targetPosition, t);
            enemyController.transform.position = currentPosition;


            yield return null; // 다음 프레임까지 대기
        }

        // 마지막 위치 설정 (정확히 목표 지점)

        enemyController.rigid.velocity = Vector3.zero;
        enemyController.transform.position = targetPosition;


    }

    private void Dash()
    {
        isAction = true;
        isHammer = false;


        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        if (!enemyController.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) enemyController.animator.Play("Dash");

        target.GetComponent<Player>().CoroutineEvent(DashCor());
    }

    private IEnumerator DashCor()
    {
        float time = 1f;

        Vector3 playerPos = target.transform.position;

        while (Vector3.Distance(target.transform.position, enemyController.transform.position) > 10f && time > 0)
        {
            time -= Time.deltaTime;

            enemyController.rigid.velocity = enemyController.transform.forward * dashSpeed;

            yield return new WaitForFixedUpdate();
        }

        HammerDown(playerPos);
    }

    private void NormalAttack()
    {
        isAction = true;
        enemyController.rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        animator.Play("Normal_Attack", 0, 0f);
        curTime = coolTime;
    }

    private void RunAttack()
    {
        isAction = true;
        enemyController.rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        animator.Play("Normal_RunAttack_1", 0, 0f);
        curTime = coolTime;
    }

    private void Attack()
    {
        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        enemyController.rigid.velocity = Vector3.zero;

        NormalAttack();
    }


    private void Move()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) animator.Play("Run", 0, 0f);

        if (!isRun) isRun = true;

        bool temp = (enemyController.rigid.constraints & RigidbodyConstraints.FreezePosition) != 0;

        if (temp) enemyController.rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;


        enemyController.transform.LookAt(target.transform);
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);

        //enemyController.rigid.AddForce(enemyController.transform.forward.normalized , ForceMode.VelocityChange);

        enemyController.rigid.velocity = enemyController.transform.forward * moveSpeed;
        //enemyController.transform.position = enemyController.transform.position + enemyController.transform.forward.normalized * moveSpeed * Time.deltaTime;
    }

    public override void GetDamage(float damage)
    {
        if (enemyController.isDead) return;
        enemyController.uiShowDelay = 4f;
        enemyController.canvas.gameObject.SetActive(true);
        int temp = Random.Range(1, 3);
        string temp2 = "GetDamage_" + temp.ToString();
        //enemyController.transform.LookAt(target.transform);
        //enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);
        //if(!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        //    animator.Play(temp2, 0, 0);
        curHp -= damage;
        backHpHit = false;
        GameManager.Instance.soundManager.Play("enemyHit", false);
        enemyController.Invoke("BackHpFunMessage", 0.3f);

        if (curHp <= 0) Die();
    }



    public override void Die()
    {
        enemyController.isDead = true;

        enemyController.gameObject.layer = enemyController.deadLayer;

        animator.Play("Dead");
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight, float speedMultiplier)
    {
        Debug.LogError("StartPoint" + startPoint);
        Debug.LogError("EndPoint" + endPoint);
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        // 궤적의 최고점 높이를 기반으로 시간을 계산
        float timeToPeak = Mathf.Sqrt(-2 * trajectoryHeight / gravity);
        float timeFromPeakToTarget = Mathf.Sqrt(2 * (trajectoryHeight - displacementY) / -gravity);
        float originalTotalTime = timeToPeak + timeFromPeakToTarget;

        // 속도 배율을 적용하여 시간을 줄임
        float totalTime = originalTotalTime / speedMultiplier;

        // 시간 값이 0이 아니도록 확인
        if (totalTime <= 0)
        {
            Debug.LogError("Total time is zero or negative, which is invalid for velocity calculation.");
            return Vector3.zero;
        }

        // XZ 평면 속도 계산
        Vector3 velocityXZ = displacementXZ / totalTime;

        // Y축 속도 계산
        float velocityYToPeak = Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        float velocityYFromPeak = Mathf.Sqrt(2 * gravity * (trajectoryHeight - displacementY));

        // 두 속도의 평균을 사용하여 총 Y축 속도를 계산
        float velocityY = (velocityYToPeak + velocityYFromPeak) / 2;

        // 속도 값이 NaN이 아닌지 확인
        if (float.IsNaN(velocityXZ.x) || float.IsNaN(velocityXZ.y) || float.IsNaN(velocityXZ.z) || float.IsNaN(velocityY))
        {
            Debug.LogError("Calculated velocity contains NaN values.");
            return Vector3.zero;
        }

        // 최종 속도 벡터 반환
        return velocityXZ + Vector3.up * velocityY;
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
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle")) animator.Play("Boss_Idle", 0, 0f);
            return;
        }

        int rand = Random.Range(0, 101);

        if (rand <= 20)
        {
            TrackingBullet();
        }
        else if (20 < rand && rand <= 40)
        {
            Bombing();
        }
        else if (40 < rand && rand <= 60)
        {
            FireWave();
        }
        else if (60 < rand && rand <= 80)
        {
            ContinuousPizza();
        }
        else if (80 < rand && rand <= 100)
        {
            RightSwing();
        }


    }

    private void RightSwing()
    {
        if (isAction) return;
        Debug.Log("RIghtSwing");
        isAction = true;
        target.GetComponent<Player>().CoroutineEvent(SwingCor());

    }


    private IEnumerator SwingCor()
    {

        //animator.Play("FireWave", 0, 0f);
        enemyController.transform.forward = target.transform.position - enemyController.transform.position;
        enemyController.transform.eulerAngles = new Vector3(0f, enemyController.transform.eulerAngles.y, 0f);


        GameObject.Instantiate(swingArea, enemyController.transform.position, enemyController.transform.rotation);
        animator.Play("Boss NormalAttack", 0, 0f);
        yield return new WaitForSeconds(0.5f);


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
        Debug.Log("ContinuousPizza");
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
        Debug.Log("FireWave");
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
        Debug.Log("Tracking");
        isAction = true;
        animator.Play("Tracking_Ready", 0, 0f);
    }

    public override void TrackingAnimExit()
    {
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
            animator.Play("Tracking_End", 0, 0f);
    }

    private void Bombing()
    {
        if (isAction) return;
        Debug.Log("Bombing");
        isAction = true;

        animator.Play("Bombing_Ready", 0, 0f);

    }

    public override void BombingAnimExit()
    {
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
                    Vector3 pos = new Vector3(enemyController.transform.position.x + Random.Range(-15f, 15f), enemyController.transform.position.y, enemyController.transform.position.z + Random.Range(-15f, 15f));
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
            animator.Play("Bombing_End", 0, 0f);
        isAction = false;
        attackCurDelay = attackDelay;
    }



    public override void GetDamage(float damage)
    {
        curHp -= damage;
        backHpHit = false;
        enemyController.anim_dot.DORestartById("Hit");
        GameManager.Instance.soundManager.Play("enemyHit", false);
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