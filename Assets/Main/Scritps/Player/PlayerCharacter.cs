using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerCharacter
{
    public Animator animator;

    public Player player;
    public EnemyController target;

    public GameObject model;
    public GameObject cutScene;
    public GameObject[] handParticle;
    public GameObject[] handTransform;


    public AnimationClip[] attackAnim;
    public AnimationClip Q_Anim;
    public AnimationClip E_Anim;
    public AnimationClip R_Anim;
    public AnimationClip followAnim;


    public bool isGrappleReady;
    public bool followEnemy;
    public bool isActionSkill;
    protected bool vectorReset;

    public int index;
    public int comboCounter;

    public float normalAttackDamage;
    public float lastClickedTime;
    public float lastComboEnd;

    [Header("Collider")]
    public AttackCol normalAttackCol;
    public AttackCol normalAttackCol_2;

    public AttackCol Q_AttackCol;
    public AttackCol E_AttackCol;
    public AttackCol R_AttackCol;

    [Header("AttackArea")]
    public float targetArea;
    public float attackArea;
    public float followSpeed;
    public float[] knockbacks;
    public float curKnockback;

    [Header("Effect")]
    public ParticleSystem curParticle;
    public ParticleSystem Right_Particle;
    public ParticleSystem E_Particle;
    public ParticleSystem Q_Particle;
    public ParticleSystem Chasing_Particle;
    public ParticleSystem[] normalAttackEffects;
    public GameObject Devastation_effect;

    [Header("Transform")]
    public Transform Q_Transform;
    public Transform E_Transform;

    public LayerMask animCheckLayer;

    public Vector3[] knockbackDirs;
    public Vector3 curKnockbackDir;
    protected Vector3 grappleVec;

    public float time;


    public virtual void Init(Player playerController)
    {
        player = playerController;
        //weapon_obj.SetActive(false);
    }

    public virtual void CharacterChange()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void LeftAction()
    {

    }

    public virtual void RightAction()
    {

    }

    public virtual void Q_Action()
    {

    }

    public virtual void E_Action()
    {

    }

    public virtual void R_Action()
    {

    }

   
   

    public virtual void Interaction()
    {
        player.targetSet.targetInteraction?.OnInteract();
    }

    public virtual void FollowExit()
    {

    }

    public virtual void NormalAttack()
    {

    }

    public virtual void SuperAttack()
    {

    }

    public virtual void AttackMotion()
    {

    }

    public virtual void FollowAttack()
    {

    }

    public virtual void FollowEnemy()
    {

    }

    public void ExitAttack()
    {
        if (player.currentCharacter.animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.9f && player.currentCharacter.animator.GetCurrentAnimatorStateInfo(3).IsTag("Attack"))
        {

            player.isAttack = false;
            player.isNormalAttack = false;
            player.Invoke("EndCombo", 0.5f);
        }
    }


    public virtual void Sound(string name)
    {
        if (name == "jump")
        {

        }
    }

    public virtual void NormalAttackExit()
    {

    }

    public virtual void ExitSuperAttack()
    {

    }


    public virtual void StrongAttackExit()
    {

    }

    public virtual void Q_AnimExit()
    {

    }

    public virtual void E_AnimExit()
    {

    }

    public virtual void R_AnimExit()
    {

    }

    public virtual float Right_Cool()
    {
        return 0;
    }

    public virtual float Q_Cool()
    {
        return 0;
    }

    public virtual float E_Cool()
    {
        return 0;
    }

    public virtual float R_Cool()
    {
        return 0;
    }

    public virtual float Change_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["CharacterChange"].curCoolTime / GameManager.Instance.coolTimeManager.coolDic["CharacterChange"].maxCoolTime;
    }

    public void RotationZero()
    {
        if (player.isSuperAttacking)
        {
            player.transform.position = player.orginPos;
            player.Rigidbody.velocity = Vector3.zero;
        }

        float x = model.transform.localEulerAngles.x;
        float y = model.transform.localEulerAngles.y;
        float z = model.transform.localEulerAngles.z;

        if (x != 0 || y != 0 || z != 0)
            model.transform.localEulerAngles = Vector3.zero;

        if (animator.GetCurrentAnimatorStateInfo(3).IsTag("Attack") && target != null && player.movementStateMachine.CurStateName() != "PlayerDashingState")
        {
            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        }
    }

    public void AnimTransform()
    {
        if (animator.GetCurrentAnimatorStateInfo(3).IsTag("Attack"))
        {
            //Debug.LogError(model.transform.localPosition);

            RaycastHit hit;

            if (Physics.Raycast(player.transform.position + Vector3.up, player.transform.forward, out hit, 0.1f, animCheckLayer))
            {
                model.transform.localPosition = new Vector3(0f, model.transform.localPosition.y, 0f);
            }

            player.transform.position = model.transform.position;
            model.transform.localPosition = Vector3.zero;
        }
        else
        {
            model.transform.localPosition = Vector3.zero;

        }
    }

    public void AnimToPos()
    {
        Debug.LogError("d");
        //player.transform.position = model.transform.position;
        model.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void HandEffectFalse()
    {
        for (int i = 0; i < handParticle.Length; i++)
        {
            handParticle[i].SetActive(false);
        }
    }


    public virtual void SkillCoolTimeResetAnim(int index)
    {

    }

    public virtual void CutSceneEvent(GameObject cut)
    {

    }

    public IEnumerator outLineCor(Outline outLine, float value, float oper, float pitch)
    {
        if (oper > 0f)
        {
            while (outLine.OutlineWidth < value)
            {
                outLine.OutlineWidth += Time.deltaTime * oper * pitch;
                yield return new WaitForEndOfFrame();
                if (!player.skillData.isHand) break;
            }
        }
        else if (oper < 0f)
        {
            while (outLine.OutlineWidth > value)
            {
                outLine.OutlineWidth += Time.deltaTime * oper * pitch;
                yield return new WaitForEndOfFrame();
            }
        }
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
public class WhiteCharacter : PlayerCharacter
{
    [SerializeField] private GameObject normal_projectile;
    [SerializeField] private GameObject super_laser;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject psionicStormEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private Transform explosionEndTransform;

    public override void Init(Player playerController)
    {
        base.Init(playerController);

    }

    public override void CharacterChange()
    {
        //PutDown();
    }

    public override void Update()
    {
        ExitAttack();
        FollowEnemy();
        AnimTransform();
        RotationZero();
    }

    public override void LeftAction()
    {
        if (player.Animator.GetBool("isDashing")) return;
        if (!player.isGround) return;
        if (GameManager.Instance.questManager.isInput)
            GameManager.Instance.questManager.InputQuestCheck(KeyCode.Mouse0);
        NormalAttack();
    }

    public override void NormalAttack()
    {
        if (followEnemy) return;

        if (player.targetSet.targetEnemy != null)
            target = player.targetSet.targetEnemy;
        player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);

        if (player.isSuperAttack && target != null)
            SuperAttack();
        else
            AttackMotion();
    }

    public override void SuperAttack()
    {
        player.isNormalAttack = true;
        player.isSuperAttacking = true;
        player.orginPos = player.transform.position;
        //handParticle[2].SetActive(true);
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.Rigidbody.velocity = Vector3.zero;

        if (target != null)
        {
            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        }
        else
        {
            player.transform.forward = Camera.main.transform.forward;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        }
        player.currentCharacter.animator.Play("White_SuperAttack", 3, 0f);
        GameManager.Instance.uiManager.ChangeAttack(0);
        followEnemy = false;
        comboCounter = 0;
    }

    public override void AttackMotion()
    {
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length && !player.isAttack && !player.isNormalAttack)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                player.isNormalAttack = true;
                player.currentCharacter.animator.Play(attackAnim[comboCounter].name, 3, 0f);
                curParticle = normalAttackEffects[comboCounter];
                curKnockback = knockbacks[comboCounter];
                curKnockbackDir = knockbackDirs[comboCounter];
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter >= attackAnim.Length)
                {
                    comboCounter = 0;
                }

                player.Rigidbody.velocity = Vector3.zero;

                if (target != null)
                {
                    player.transform.forward = target.transform.position - player.transform.position;
                    player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
                }
            }
        }
    }

    public override void Q_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("White_Q")) return;
        GameManager.Instance.coolTimeManager.GetCoolTime("White_Q");
        player.isAttack = true;
        ExplosionAnim();
    }

    private void ExplosionAnim()
    {
        GameObject temp = GameObject.Instantiate(cutScene, player.transform.position, Quaternion.identity);
        temp.SetActive(true);
    }

    private void Explosion()
    {
        player.Rigidbody.velocity = Vector3.zero;
        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        //OnlySingleton.Instance.white_Q_cam.Priority = 11;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[4];
        curKnockbackDir = knockbackDirs[4];

        player.currentCharacter.animator.Play(Q_Anim.name, 3, 0f);
    }

    public override void E_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("White_E")) return;
        GameManager.Instance.coolTimeManager.GetCoolTime("White_E");

        PsionicStorm();
    }

    public override void CutSceneEvent(GameObject cut)
    {
        GameObject.Destroy(cut);
        Explosion();
    }

    private void PsionicStorm()
    {
        GameObject temp = GameObject.Instantiate(psionicStormEffect, player.transform.position, Quaternion.identity);
        temp.SetActive(true);
    }

    public override void NormalAttackExit()
    {
        player.isNormalAttack = false;
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        if (target != null)
        {
            Player.Instance.StartCoroutine(ProjectileCor());
        }

        GameObject temp_2 = GameObject.Instantiate(fireEffect, fireTransform.position, Quaternion.identity);
        temp_2.transform.forward = player.transform.forward;

    }

    public override void ExitSuperAttack()
    {
        player.isNormalAttack = false;
        player.isSuperAttacking = false;
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        if (target != null)
        {
            SuperProjectile();
        }

        GameObject temp_2 = GameObject.Instantiate(fireEffect, fireTransform.position, Quaternion.identity);
        temp_2.transform.forward = player.transform.forward;
        player.isSuperAttack = false;
    }

    private void SuperProjectile()
    {
        if (target != null)
        {
            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
            GameObject temp = GameObject.Instantiate(super_laser, fireTransform.position, Quaternion.identity);
            temp.GetComponent<White_projectile>().target = target.transform;
            temp.gameObject.SetActive(true);
        }
    }

    private IEnumerator ProjectileCor()
    {
        for (int i = 0; i < 3; i++)
        {
            if (target != null)
            {
                GameObject temp = GameObject.Instantiate(normal_projectile, fireTransform.position, Quaternion.identity);
                temp.GetComponent<White_projectile>().target = target.transform;
                temp.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void StrongAttackExit()
    {
        base.StrongAttackExit();
    }

    public override void Q_AnimExit()
    {
        //OnlySingleton.Instance.white_Q_cam.Priority = 9;

        //Vector3 pos = player.transform.position + player.transform.forward * 10f;
        //OnlySingleton.Instance.camShake.ShakeCamera(7f, 0.1f);
        if (target != null)
        {
            GameObject temp = GameObject.Instantiate(explosionEffect, target.transform.position, Quaternion.identity);
            temp.SetActive(true);
        }
        else
        {
            GameObject temp = GameObject.Instantiate(explosionEffect, explosionEndTransform.position, Quaternion.identity);
            temp.SetActive(true);
        }

        GameManager.Instance.coolTimeManager.GetCoolTime("CharacterChange");

    }

    public override void E_AnimExit()
    {
        base.E_AnimExit();
    }

    public override float Q_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["White_Q"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["White_Q"].maxCoolTime;
    }

    public override float E_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["White_E"].curCoolTime /
           GameManager.Instance.coolTimeManager.coolDic["White_E"].maxCoolTime;
    }

    public override float Change_Cool()
    {
        return base.Change_Cool();
    }

    public override void SkillCoolTimeResetAnim(int index)
    {
        if (index == 0)
        {
            GameManager.Instance.uiManager.Q_Skill_Icon.SetActive(true);
            GameManager.Instance.uiManager.Q_Skill_Icon.GetComponent<Animator>().Play("Effect", 0);
        }
        else if (index == 1)
        {
            GameManager.Instance.uiManager.E_Skill_Icon.SetActive(true);
            GameManager.Instance.uiManager.E_Skill_Icon.GetComponent<Animator>().Play("Effect", 0);

        }
    }

    public override void Sound(string name)
    {
        if (name == "jump")
        {
            GameManager.Instance.soundManager.Play("red_jump", false);
        }
    }

    #region No

    public override void R_Action()
    {

    }

    public override void RightAction()
    {

        //if (player.skillData.isHand) Throw();
        //else Catch();
    }

    public override float R_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["White_R"].curCoolTime /
           GameManager.Instance.coolTimeManager.coolDic["White_R"].maxCoolTime;
    }

    public override void R_AnimExit()
    {
        base.R_AnimExit();
    }

    public override float Right_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["White_Right"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["White_Right"].maxCoolTime;
    }
    #endregion
}

[System.Serializable]
public class GreenCharacter : PlayerCharacter
{
    [SerializeField] private GameObject dragon_projectile;

    public override void Init(Player playerController)
    {
        base.Init(playerController);

    }

    public override void CharacterChange()
    {

    }

    public override void Update()
    {
        ExitAttack();
        FollowEnemy();
        AnimTransform();
        RotationZero();

    }

    public override void LeftAction()
    {
        //if (!player.isGround) StartGrappleAnim();

        if (player.Animator.GetBool("isDashing")) return;
        if (!player.isGround) return;
        if (GameManager.Instance.questManager.isInput)
            GameManager.Instance.questManager.InputQuestCheck(KeyCode.Mouse0);
        NormalAttack();
    }

    public override void Q_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Green_Q")) return;
        if (followEnemy) return;
        GameManager.Instance.coolTimeManager.GetCoolTime("Green_Q");
        player.isAttack = true;
        DragonAnim();
    }

    private void DragonAnim()
    {
        GameObject temp = GameObject.Instantiate(cutScene, player.transform.position, Quaternion.identity);
        temp.SetActive(true);
    }

    public override void CutSceneEvent(GameObject cut)
    {
        GameObject.Destroy(cut);
        Dragon();
    }

    private void Dragon()
    {
        player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
        player.Rigidbody.velocity = Vector3.zero;

        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        //OnlySingleton.Instance.green_E_cam.Priority = 11;
        player.Rigidbody.useGravity = false;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[3];
        curKnockbackDir = knockbackDirs[3];

        player.currentCharacter.animator.Play(Q_Anim.name, 3, 0f);
    }

    public override void E_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Green_E")) return;
        if (followEnemy) return;
        GameManager.Instance.coolTimeManager.GetCoolTime("Green_E");
        player.isAttack = true;
        Gust();
    }

    private void Gust()
    {
        player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
        player.Rigidbody.velocity = Vector3.zero;
        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        OnlySingleton.Instance.green_E_cam.Priority = 11;
        player.Rigidbody.useGravity = false;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[3];
        curKnockbackDir = knockbackDirs[3];

        player.currentCharacter.animator.Play(E_Anim.name, 3, 0f);
    }

    public override void NormalAttackExit()
    {
        player.isNormalAttack = false;
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        normalAttackCol.gameObject.SetActive(true);
    }

    public override void StrongAttackExit()
    {
        player.isNormalAttack = false;
        OnlySingleton.Instance.camShake.ShakeCamera(7f, 0.1f);
        GameManager.Instance.soundManager.Play("red_normalAttack2", false);
        normalAttackCol_2.gameObject.SetActive(true);
    }

    public override void Q_AnimExit()
    {
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        player.isAttack = false;
        GameObject.Instantiate(Q_Particle.gameObject, model.transform.position, Quaternion.identity);
        player.Rigidbody.useGravity = true;
        GameObject temp = GameObject.Instantiate(dragon_projectile, model.transform.position + Vector3.up, Quaternion.identity);
        temp.transform.forward = player.transform.forward;
        GameManager.Instance.coolTimeManager.GetCoolTime("CharacterChange");
        temp.gameObject.SetActive(true);
    }

    public override void E_AnimExit()
    {
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        player.isAttack = false;
        OnlySingleton.Instance.green_E_cam.Priority = 9;
        GameObject temp = GameObject.Instantiate(E_Particle.gameObject, model.transform.position, Quaternion.identity);
        temp.transform.forward = player.transform.forward;
        player.Rigidbody.useGravity = true;
        E_AttackCol.gameObject.SetActive(true);
    }

    public override void FollowEnemy()
    {

        if (!followEnemy) return;

        if (target == null) { followEnemy = false; return; };


        if (target != null && Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
        {
            time -= Time.deltaTime;
            player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
            if (!animator.GetCurrentAnimatorStateInfo(3).IsName(followAnim.name))
            {
                player.currentCharacter.animator.Play(followAnim.name, 3, 0f);
                GameObject.Instantiate(Chasing_Particle.gameObject, model.transform.position + Vector3.up, model.transform.rotation);
            }

            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);

            RaycastHit hit;

            if (time < 0)
            {
                FollowExit();
                return;
            }

            if (target != null && Physics.Raycast(player.transform.position, player.transform.forward, out hit, Vector3.Distance(target.transform.position, player.transform.position), player.LayerData.GroundLayer))
            {
                FollowExit();
                return;
            }



            player.Rigidbody.velocity = Vector3.zero;
            player.Rigidbody.AddForce(player.transform.forward * followSpeed, ForceMode.VelocityChange);
        }
        else
        {
            FollowExit();
        }
    }

    public override void FollowExit()
    {
        //AttackMotion = null;
        if (player.isSuperAttack)
            SuperAttack();
        else
            FollowAttack();
    }

    public override void FollowAttack()
    {
        player.isNormalAttack = true;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.Rigidbody.velocity = Vector3.zero;
        player.transform.forward = target.transform.position - player.transform.position;
        player.currentCharacter.animator.Play(attackAnim[2].name, 3, 0f);
        followEnemy = false;
        comboCounter = 0;

    }

    public override void ExitSuperAttack()
    {
        player.isNormalAttack = false;
        player.isSuperAttack = false;
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        normalAttackCol.gameObject.SetActive(true);
        player.isSuperAttacking = false;
    }

    public override void NormalAttack()
    {
        if (followEnemy) return;

        if (player.targetSet.targetEnemy != null)
        {

            target = player.targetSet.targetEnemy;
            if (Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
            {
                followEnemy = true;
                time = 0.5f;
            }
            else
            {
                if (player.isSuperAttack && target != null) SuperAttack();
                else
                    AttackMotion();
            }

        }
        else
        {
            if (player.isSuperAttack && target != null) SuperAttack();
            else
                AttackMotion();
        }
    }

    public override void SuperAttack()
    {
        player.isNormalAttack = true;
        player.isSuperAttacking = true;
        player.orginPos = player.transform.position;
        //handParticle[2].SetActive(true);
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.Rigidbody.velocity = Vector3.zero;

        player.transform.forward = target.transform.position - player.transform.position;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);

        player.currentCharacter.animator.Play("Green_SuperAttack", 3, 0f);
        GameManager.Instance.uiManager.ChangeAttack(1);
        followEnemy = false;
        comboCounter = 0;
    }

    public override void AttackMotion()
    {
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length && !player.isNormalAttack && !player.isAttack)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                player.isNormalAttack = true;
                player.currentCharacter.animator.Play(attackAnim[comboCounter].name, 3, 0f);
                curParticle = normalAttackEffects[comboCounter];
                curKnockback = knockbacks[comboCounter];
                curKnockbackDir = knockbackDirs[comboCounter];
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter >= attackAnim.Length)
                {
                    comboCounter = 0;
                }

                player.Rigidbody.velocity = Vector3.zero;

                if (target != null)
                {
                    player.transform.forward = target.transform.position - player.transform.position;
                    player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
                }
            }
        }
    }

    public override void Sound(string name)
    {
        if (name == "jump")
        {
            GameManager.Instance.soundManager.Play("red_jump", false);
        }
    }

    public override float Q_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Green_Q"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Green_Q"].maxCoolTime;
    }

    public override float E_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Green_E"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Green_E"].maxCoolTime;
    }

    public override float Change_Cool()
    {
        return base.Change_Cool();
    }

    public override void SkillCoolTimeResetAnim(int index)
    {
        if (index == 2)
        {
            GameManager.Instance.uiManager.Q_Skill_Icon.SetActive(true);
            GameManager.Instance.uiManager.Q_Skill_Icon.GetComponent<Animator>().Play("Effect", 0);
        }
        else if (index == 3)
        {
            GameManager.Instance.uiManager.E_Skill_Icon.SetActive(true);
            GameManager.Instance.uiManager.E_Skill_Icon.GetComponent<Animator>().Play("Effect", 0);

        }
    }

    #region No
    public override void R_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Green_R")) return;

        GameManager.Instance.coolTimeManager.GetCoolTime("Green_R");
    }

    public override float Right_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Green_Right"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Green_Right"].maxCoolTime;
    }

    public override float R_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Green_R"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Green_R"].maxCoolTime;
    }

    public override void RightAction()
    {
        base.RightAction();
    }

    #endregion
}

[System.Serializable]
public class RedCharacter : PlayerCharacter
{
    public override void Init(Player playerController)
    {
        base.Init(playerController);

    }

    public override void CharacterChange()
    {

    }

    public override void Update()
    {
        ExitAttack();
        FollowEnemy();
        RotationZero();
        AnimTransform();

    }

    public override void LeftAction()
    {
        if (player.Animator.GetBool("isDashing")) return;
        if (!player.isGround) return;
        if (GameManager.Instance.questManager.isInput)
            GameManager.Instance.questManager.InputQuestCheck(KeyCode.Mouse0);
        NormalAttack();
    }

    public override void Q_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Red_Q")) return;
        if (followEnemy) return;
        GameManager.Instance.coolTimeManager.GetCoolTime("Red_Q");
        player.isAttack = true;
        DevastationAnim();
    }

    public override void E_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Red_E")) return;
        if (followEnemy) return;
        GameManager.Instance.coolTimeManager.GetCoolTime("Red_E");
        player.isAttack = true;
        HellDive();
    }

    private void HellDive()
    {
        player.Rigidbody.velocity = Vector3.zero;
        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        OnlySingleton.Instance.red_E_cam.Priority = 11;
        player.Rigidbody.useGravity = false;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[3];
        curKnockbackDir = knockbackDirs[3];

        player.currentCharacter.animator.Play(E_Anim.name, 3, 0f);
    }

    private void DevastationAnim()
    {
        GameObject temp = GameObject.Instantiate(cutScene, player.transform.position, Quaternion.identity);
        temp.SetActive(true);
    }

    public override void CutSceneEvent(GameObject cut)
    {
        GameObject.Destroy(cut);
        Devastation();
    }

    private void Devastation()
    {
        player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
        isActionSkill = true;
        player.Rigidbody.velocity = Vector3.zero;
        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[4];
        curKnockbackDir = knockbackDirs[4];
        player.currentCharacter.animator.Play(Q_Anim.name, 3, 0f);
    }

    public override void FollowEnemy()
    {
        if (!followEnemy) return;

        if (target == null) { followEnemy = false; return; };


        if (target != null && Vector3.Distance(target.transform.position, player.transform.position) > attackArea - 2f)
        {
            time -= Time.deltaTime;
            player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
            if (!animator.GetCurrentAnimatorStateInfo(3).IsName(followAnim.name))
            {
                player.currentCharacter.animator.Play(followAnim.name, 3, 0f);
                GameObject.Instantiate(Chasing_Particle.gameObject, model.transform.position + Vector3.up, model.transform.rotation);
            }

            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);

            RaycastHit hit;

            if (time < 0)
            {
                FollowExit();
                return;
            }

            if (target != null && Physics.Raycast(player.transform.position, player.transform.forward, out hit, Vector3.Distance(target.transform.position, player.transform.position), player.LayerData.GroundLayer))
            {
                FollowExit();
                return;
            }



            player.Rigidbody.velocity = Vector3.zero;
            player.Rigidbody.AddForce(player.transform.forward * followSpeed, ForceMode.VelocityChange);
        }
        else
        {
            FollowExit();
        }
    }

    public override void FollowExit()
    {
        //AttackMotion = null;
        if (player.isSuperAttack)
            SuperAttack();
        else
            FollowAttack();
    }

    public override void FollowAttack()
    {

        player.isNormalAttack = true;
        handParticle[2].SetActive(true);
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.Rigidbody.velocity = Vector3.zero;
        player.transform.forward = target.transform.position - player.transform.position;
        player.currentCharacter.animator.Play(attackAnim[2].name, 3, 0f);
        followEnemy = false;
        comboCounter = 0;
    }

    public override void NormalAttackExit()
    {
        player.isNormalAttack = false;
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        normalAttackCol.gameObject.SetActive(true);
    }

    public override void ExitSuperAttack()
    {
        player.isNormalAttack = false;
        player.isSuperAttack = false;
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        normalAttackCol.gameObject.SetActive(true);
        player.isSuperAttacking = false;
    }

    public override void StrongAttackExit()
    {
        player.isNormalAttack = false;
        OnlySingleton.Instance.camShake.ShakeCamera(7f, 0.1f);
        GameManager.Instance.soundManager.Play("red_normalAttack2", false);
        normalAttackCol_2.gameObject.SetActive(true);
    }

    public override void Q_AnimExit()
    {
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        GameObject temp = GameObject.Instantiate(Devastation_effect, model.transform.position, Quaternion.identity);
        temp.transform.forward = player.transform.forward;
        player.isAttack = false;
        GameManager.Instance.coolTimeManager.GetCoolTime("CharacterChange");
        Q_AttackCol.gameObject.SetActive(true);
    }

    public override void E_AnimExit()
    {
        GameManager.Instance.soundManager.Play("red_normalAttack1", false);
        player.isAttack = false;
        OnlySingleton.Instance.red_E_cam.Priority = 9;
        GameObject temp = GameObject.Instantiate(E_Particle.gameObject, model.transform.position, Quaternion.identity);
        temp.transform.forward = player.transform.forward;
        player.Rigidbody.useGravity = true;
        E_AttackCol.gameObject.SetActive(true);
    }

    public override void AttackMotion()
    {
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length && !player.isAttack && !player.isNormalAttack)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                player.isNormalAttack = true;
                player.currentCharacter.animator.Play(attackAnim[comboCounter].name, 3, 0f);
                handParticle[comboCounter].SetActive(true);
                //GameObject temp = GameObject.Instantiate(handParticle[comboCounter], handTransform[comboCounter].transform);
                //temp.transform.position = temp.transform.parent.transform.position;
                //temp.SetActive(true);
                curParticle = normalAttackEffects[comboCounter];
                curKnockback = knockbacks[comboCounter];
                curKnockbackDir = knockbackDirs[comboCounter];
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter >= attackAnim.Length)
                {
                    comboCounter = 0;
                }

                player.Rigidbody.velocity = Vector3.zero;

                if (target != null)
                {
                    player.transform.forward = target.transform.position - player.transform.position;
                    player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
                }

                //player.Rigidbody.AddForce(player.transform.forward * 10f, ForceMode.VelocityChange);
            }
        }

    }

    public override void NormalAttack()
    {
        if (followEnemy) return;

        if (player.targetSet.targetEnemy != null)
        {

            target = player.targetSet.targetEnemy;
            if (Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
            {
                followEnemy = true;
                time = 0.5f;
            }
            else
            {
                if (player.isSuperAttack && target != null) SuperAttack();
                else
                    AttackMotion();
            }
        }
        else
        {
            if (player.isSuperAttack && target != null) SuperAttack();
            else
                AttackMotion();
        }
    }

    public override void SuperAttack()
    {

        player.isNormalAttack = true;
        player.isSuperAttacking = true;
        player.orginPos = player.transform.position;
        handParticle[2].SetActive(true);
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.Rigidbody.velocity = Vector3.zero;

        player.transform.forward = target.transform.position - player.transform.position;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);


        player.currentCharacter.animator.Play("Red_SuperAttack", 3, 0f);
        GameManager.Instance.uiManager.ChangeAttack(2);
        followEnemy = false;
        comboCounter = 0;
    }

    public override float Q_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Red_Q"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Red_Q"].maxCoolTime;
    }

    public override float E_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Red_E"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Red_E"].maxCoolTime;
    }

    public override float Change_Cool()
    {
        return base.Change_Cool();
    }

    public override void SkillCoolTimeResetAnim(int index)
    {
        if (index == 4)
        {
            GameManager.Instance.uiManager.Q_Skill_Icon.SetActive(true);
            GameManager.Instance.uiManager.Q_Skill_Icon.GetComponent<Animator>().Play("Effect", 0);
        }
        else if (index == 5)
        {
            GameManager.Instance.uiManager.E_Skill_Icon.SetActive(true);
            GameManager.Instance.uiManager.E_Skill_Icon.GetComponent<Animator>().Play("Effect", 0);

        }
    }

    public override void Sound(string name)
    {
        if (name == "jump")
        {
            GameManager.Instance.soundManager.Play("red_jump", false);
        }
    }

    #region No
    public override void R_Action()
    {

    }

    public override void R_AnimExit()
    {

    }

    public override float R_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Red_R"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Red_R"].maxCoolTime;
    }

    public override float Right_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Red_Right"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Red_Right"].maxCoolTime;
    }

    public override void RightAction()
    {
        base.RightAction();
    }
    #endregion
}

