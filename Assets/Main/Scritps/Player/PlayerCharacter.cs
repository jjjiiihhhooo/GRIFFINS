using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerCharacter
{
    public Animator animator;

    public Player player;
    public EnemyController target;

    public GameObject model;

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

    [Header("Transform")]
    public Transform Q_Transform;
    public Transform E_Transform;

    public LayerMask animCheckLayer;

    public Vector3[] knockbackDirs;
    public Vector3 curKnockbackDir;
    protected Vector3 grappleVec;


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

    public virtual void StartGrapple()
    {

        player.skillData.grappling = true;
        player.freeze = true;
        player.Rigidbody.useGravity = true;
        player.Rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        //player.skillData.lr.enabled = true;
        //player.skillData.lr.SetPosition(0, model.transform.position);
        //player.skillData.lr.SetPosition(1, player.skillData.grapplePoint);
        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, player.skillData.maxGrappleDistance, player.skillData.grappleMask))
        //{
        //    player.skillData.grapplePoint = hit.point;
        //    //player.CoroutineEvent(DelayCor(player.skillData.grappleDelayTime, 0));
        //    player.StartCoroutine(DelayCor(player.skillData.grappleDelayTime, 0));
        //}
        //else
        //{
        //    //player.CoroutineEvent(DelayCor(player.skillData.grappleDelayTime, 1));
        //    player.StartCoroutine(DelayCor(player.skillData.grappleDelayTime, 1));
        //}
        //player.skillData.lr.gameObject.SetActive(true);

        ExecuteGrapple();
    }

    public IEnumerator DelayCor(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        if (index == 0)
            ExecuteGrapple();
        else if (index == 1)
            StopGrapple();
    }

    public virtual void ExecuteGrapple()
    {

        player.freeze = false;
        player.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition;

        player.currentCharacter.animator.Play("chardash", 2, 0f);
        //player.skillData.grapplingCdTimer = player.skillData.grapplingCd;
        Vector3 lowestPoint = new Vector3(player.transform.position.x, player.transform.position.y - 1f, player.transform.position.z);



        float grapplePointRelativeYPos = player.skillData.grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + player.skillData.overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = player.skillData.overshootYAxis;

        player.skillData.velocity = CalculateJumpVelocity(player.transform.position, player.skillData.grapplePoint, highestPointOnArc);
        player.ResizableCapsuleCollider.SlopeData.StepHeightPercentage = 0f;
        player.skillData.touch = true;
        // player.skillData.lr.gameObject.SetActive(false);
        //player.GetComponent<Rigidbody>().velocity = player.skillData.velocity;

        player.Rigidbody.AddForce(grappleVec * player.skillData.grappleSpeed, ForceMode.Impulse);
    }

    public virtual void StopGrapple()
    {
        player.freeze = false;
        player.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition;

        player.ResizableCapsuleCollider.SlopeData.StepHeightPercentage = 0.25f;
        player.skillData.grappling = false;
        //player.skillData.grapplingCdTimer = player.skillData.grapplingCd;

        player.skillData.lr.enabled = false;
    }

    public virtual void Interaction()
    {
        player.targetSet.targetInteraction?.OnInteract();
    }

    public virtual void ItemSave()
    {

    }

    public virtual void FollowExit()
    {

    }

    public virtual void NormalAttack()
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
        float x = model.transform.localEulerAngles.x;
        float y = model.transform.localEulerAngles.y;
        float z = model.transform.localEulerAngles.z;

        if (x != 0 || y != 0 || z != 0)
            model.transform.localEulerAngles = Vector3.zero;
    }

    public void AnimTransform()
    {
        if (animator.GetCurrentAnimatorStateInfo(3).IsTag("Attack"))
        {
            //Debug.LogError(model.transform.localPosition);

            RaycastHit hit;

            if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 3f, animCheckLayer))
            {
                model.transform.localPosition = new Vector3(0f, model.transform.localPosition.y, 0f);
            }

            player.transform.position = model.transform.position;
            model.transform.localPosition = Vector3.zero;
        }
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
        RotationZero();
    }

    public override void LeftAction()
    {

    }



    public override void RightAction()
    {

        //if (player.skillData.isHand) Throw();
        //else Catch();
    }


    public override void NormalAttack()
    {

    }

    public override void AttackMotion()
    {

    }

    public override void Q_Action()
    {

    }

    public override void E_Action()
    {

    }

    public override void R_Action()
    {

    }


    public override void ItemSave()
    {
        if (player.skillData.isHand && !player.isItemSave) Save();
        else if (!player.skillData.isHand && player.isItemSave) Load();
    }

    private void Save()
    {
        player.isItemSave = true;
        player.saveItem = GameObject.Instantiate(player.skillData.handObj);
        player.saveItem.SetActive(false);
        PutDown(true);
    }

    private void Load()
    {
        if (!player.isGround) return;
        player.isItemSave = false;
        Catch(true);
        player.saveItem = null;
    }

    private void PutDown(bool save = false)
    {
        if (!player.skillData.isHand || player.skillData.handObj == null) return;
        GameManager.Instance.uiManager.crossHair.SetActive(false);
        GameManager.Instance.staminaManager.staminaImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(62f, 35.4f, 0f);
        Rigidbody rigid = player.skillData.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = player.skillData.camZoom;
        Outline outLine = player.skillData.handObj.GetComponent<Outline>();

        player.movementStateMachine.ReusableData.ShouldWalk = false;

        player.currentCharacter.animator.SetBool("isHand", false);
        rigid.isKinematic = false;
        rigid.useGravity = true;
        player.skillData.handObj.transform.SetParent(null);

        player.CoroutineEvent(outLineCor(outLine, 0f, -1f, 10f));
        player.skillData.handObj.GetComponent<BoxCollider>().isTrigger = false;

        camZoom.minimumDistance = 6f;
        camZoom.maximumDistance = 6f;
        camZoom.transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(() => camZoom.minimumDistance = 1f);

        player.skillData.lookatTransform.localPosition = new Vector3(0, 1.23f, 0f);

        if (player.movementStateMachine.ReusableData.MovementInput != Vector2.zero && player.isGround) player.movementStateMachine.ChangeState(player.movementStateMachine.RunningState);

        player.skillData.isHand = false;
        player.Rigidbody.velocity = Vector3.zero;

        if (save)
        {
            GameObject.Destroy(player.skillData.handObj);
        }

        player.skillData.handObj = null;
    }

    private void Catch(bool load = false)
    {
        if (!load)
        {
            if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
            if (player.targetSet.targetObject == null || player.skillData.isHand || player.skillData.handObj != null || !player.isGround) return;
        }

        if (player.currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") || player.currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw")) return;
        GameManager.Instance.uiManager.crossHair.SetActive(true);

        GameManager.Instance.staminaManager.staminaImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(140f, 35.4f, 0f);
        //GameManager.Instance.staminaManager.staminaImage.rectTransform.anchoredPosition = new Vector3(140f, 35.4f, 0f);

        player.currentCharacter.animator.SetBool("isHand", true);

        if (!load)
        {
            player.skillData.handObj = player.targetSet.targetObject;
        }
        else
        {
            player.skillData.handObj = player.saveItem;
            player.skillData.handObj.SetActive(true);
        }

        player.skillData.handObj.GetComponent<Collider>().isTrigger = true;


        Rigidbody rigid = player.skillData.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = player.skillData.camZoom;
        Outline outLine = player.skillData.handObj.GetComponent<Outline>();

        player.skillData.isHand = true;
        rigid.useGravity = false;
        rigid.isKinematic = true;
        player.skillData.handObj.transform.DOKill(false);

        if (!load)
            player.skillData.handObj.transform.DOMove(player.skillData.catchTransform.position, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => { player.skillData.handObj.transform.position = player.skillData.catchTransform.position; camZoom.maximumDistance = 2f; player.skillData.lookatTransform.localPosition = new Vector3(0.75f, 1.41f, 0f); player.skillData.handObj.transform.SetParent(player.transform); });
        else
            player.skillData.handObj.transform.DOMove(player.skillData.catchTransform.position, 0.01f).SetEase(Ease.OutQuad).OnComplete(() => { player.skillData.handObj.transform.position = player.skillData.catchTransform.position; camZoom.maximumDistance = 2f; player.skillData.lookatTransform.localPosition = new Vector3(0.75f, 1.41f, 0f); player.skillData.handObj.transform.SetParent(player.transform); });

        player.movementStateMachine.ReusableData.ShouldWalk = true;
        player.movementStateMachine.ReusableData.ShouldSprint = false;

        if (player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.WalkingState);
        else player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
        outLine.DOKill(false);

        //outLine.OutlineWidth = 10f;
        player.CoroutineEvent(outLineCor(outLine, 10f, 1f, 10f));
    }

    private void Throw()
    {
        if (!player.skillData.isHand || player.skillData.handObj == null) return;
        GameManager.Instance.uiManager.crossHair.SetActive(false);
        GameManager.Instance.staminaManager.staminaImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(62f, 35.4f, 0f);
        Rigidbody rigid = player.skillData.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = player.skillData.camZoom;
        Outline outLine = player.skillData.handObj.GetComponent<Outline>();

        player.movementStateMachine.ReusableData.ShouldWalk = false;
        if (player.skillData.handObj.gameObject.name == "Stamina_obj")
        {
            player.currentCharacter.animator.SetBool("isHand", false);
            GameManager.Instance.staminaManager.PlusStamina(30f);
            player.DestroyEvent(player.skillData.handObj.gameObject);
        }
        else
        {
            player.currentCharacter.animator.SetTrigger("isThrow");
            player.currentCharacter.animator.SetBool("isHand", false);
            GameManager.Instance.staminaManager.MinusStamina(20f);

            rigid.isKinematic = false;
            Vector3 aimDir = (GameManager.Instance.inputData.MouseWorldPosition - player.skillData.catchTransform.position).normalized;
            GameObject temp = player.InstantiateEvent(GameManager.Instance.inputData.projectTile, player.skillData.catchTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
            player.skillData.handObj.transform.SetParent(temp.transform);

            player.CoroutineEvent(outLineCor(outLine, 0f, -1f, 10f));
        }

        player.skillData.handObj.GetComponent<BoxCollider>().isTrigger = false;

        camZoom.minimumDistance = 6f;
        camZoom.maximumDistance = 6f;
        camZoom.transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(() => camZoom.minimumDistance = 1f);

        player.skillData.lookatTransform.localPosition = new Vector3(0, 1.23f, 0f);

        if (player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.RunningState);

        player.skillData.isHand = false;
        player.Rigidbody.velocity = Vector3.zero;
        player.skillData.handObj = null;
    }

    private void GCatch()
    {
        if (player.targetSet.grappleTargetObject == null || player.skillData.isGHand || player.skillData.GhandObj != null) return;
        if (player.skillData.grapplingCdTimer > 0) return;

        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);


        player.skillData.GhandObj = player.targetSet.grappleTargetObject;

        player.skillData.Gjoint = player.skillData.GhandObj.gameObject.AddComponent<SpringJoint>();
        player.skillData.Gjoint.autoConfigureConnectedAnchor = false;
        //skill.Gjoint.connectedAnchor = transform.position;

        player.skillData.distanceFromPoint = Vector3.Distance(player.transform.position, player.skillData.GhandObj.transform.position);

        // the distance grapple will try to keep from grapple point. 
        player.skillData.Gjoint.maxDistance = player.skillData.distanceFromPoint * 0.8f;
        player.skillData.Gjoint.minDistance = player.skillData.distanceFromPoint * 0.25f;

        // customize values as you like
        player.skillData.Gjoint.spring = 4.5f;
        player.skillData.Gjoint.damper = 7f;
        player.skillData.Gjoint.massScale = 4.5f;

        player.skillData.isGHand = true;

        player.skillData.Glr.enabled = true;
    }

    private void GPull()
    {
        player.DestroyEvent(player.skillData.Gjoint);

        Vector3 lowestPoint = new Vector3(player.skillData.GhandObj.transform.position.x, player.skillData.GhandObj.transform.position.y - 1f, player.skillData.GhandObj.transform.position.z);


        float grapplePointRelativeYPos = player.transform.position.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + player.skillData.overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = player.skillData.overshootYAxis;

        player.skillData.Gvelocity = CalculateJumpVelocity(player.skillData.GhandObj.transform.position, player.transform.position, highestPointOnArc);
        player.skillData.isGHand = false;
        player.skillData.GhandObj.transform.GetComponent<Rigidbody>().velocity = player.skillData.Gvelocity;
        player.skillData.Glr.enabled = false;
        player.skillData.GhandObj = null;
    }

    public override void NormalAttackExit()
    {
        base.NormalAttackExit();
    }

    public override void StrongAttackExit()
    {
        base.StrongAttackExit();
    }

    public override void Q_AnimExit()
    {
        base.Q_AnimExit();
    }

    public override void E_AnimExit()
    {
        base.E_AnimExit();
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

    public override float R_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["White_R"].curCoolTime /
           GameManager.Instance.coolTimeManager.coolDic["White_R"].maxCoolTime;
    }

    public override float Change_Cool()
    {
        return base.Change_Cool();
    }
}

[System.Serializable]
public class GreenCharacter : PlayerCharacter
{
    public override void Init(Player playerController)
    {
        base.Init(playerController);

    }

    public override void CharacterChange()
    {
        GameManager.Instance.predictionHitTransform.gameObject.SetActive(false);
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
        NormalAttack();
    }

    private void StartGrappleAnim()
    {
        if (isGrappleReady) return;
        if (player.skillData.grapplingCdTimer > 0) return;
        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);
        player.skillData.grapplingCdTimer = player.skillData.grapplingCd;
        player.Rigidbody.velocity = Vector3.zero;
        isGrappleReady = true;
        player.movementStateMachine.ChangeState(player.movementStateMachine.FallingState);
        player.Rigidbody.useGravity = false;
        player.transform.rotation = player.CameraRecenteringUtility.VirtualCamera.transform.rotation;
        player.transform.rotation = new Quaternion(0f, player.CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, player.transform.rotation.w);
        grappleVec = Camera.main.transform.forward;

        player.skillData.grapplePoint = model.transform.position + grappleVec * player.skillData.maxGrappleDistance;
        //player.skillData.lr.enabled = false;
        player.currentCharacter.animator.Play("GrappleReady", 2, 0f);
    }

    public override void RightAction()
    {
        base.RightAction();
    }

    public override void StartGrapple()
    {
        base.StartGrapple();
    }

    public override void ExecuteGrapple()
    {
        base.ExecuteGrapple();
    }

    public override void StopGrapple()
    {
        base.StopGrapple();
    }

    public override void Q_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Green_Q")) return;

        GameManager.Instance.coolTimeManager.GetCoolTime("Green_Q");
    }



    public override void E_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Green_E")) return;

        GameManager.Instance.coolTimeManager.GetCoolTime("Green_E");
        Gust();
    }

    private void Gust()
    {
        player.Rigidbody.velocity = Vector3.zero;
        player.isAttack = true;
        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        OnlySingleton.Instance.green_E_cam.Priority = 11;
        player.Rigidbody.useGravity = false;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[3];
        curKnockbackDir = knockbackDirs[3];

        player.currentCharacter.animator.Play(E_Anim.name, 3, 0f);
    }

    public override void E_AnimExit()
    {
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack1"], false);
        player.isAttack = false;
        OnlySingleton.Instance.green_E_cam.Priority = 9;
        GameObject.Instantiate(E_Particle.gameObject, model.transform.position, Quaternion.identity);
        player.Rigidbody.useGravity = true;
        E_AttackCol.gameObject.SetActive(true);
    }

    public override void R_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Green_R")) return;

        GameManager.Instance.coolTimeManager.GetCoolTime("Green_R");
    }

    public override void FollowEnemy()
    {
        if (!followEnemy) return;

        if (target == null) { followEnemy = false; return; };


        if (Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
        {
            if (!animator.GetCurrentAnimatorStateInfo(3).IsName(followAnim.name))
            {
                player.currentCharacter.animator.Play(followAnim.name, 3, 0f);
                GameObject.Instantiate(Chasing_Particle.gameObject, model.transform.position + Vector3.up, model.transform.rotation);
            }

            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);

            RaycastHit hit;

            if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, Vector3.Distance(target.transform.position, player.transform.position), player.skillData.grappleMask))
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
        FollowAttack();
    }

    public override void FollowAttack()
    {
        player.isAttack = true;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.currentCharacter.animator.Play(attackAnim[2].name, 3, 0f);
        followEnemy = false;
        comboCounter = 0;
    }

    public override void NormalAttack()
    {
        if (followEnemy) return;

        if (player.targetSet.targetEnemy != null)
        {

            target = player.targetSet.targetEnemy;
            if (Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
                followEnemy = true;
            else AttackMotion();
        }
        else
        {
            AttackMotion();
        }
    }

    public override void AttackMotion()
    {
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length && !player.isAttack)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                player.isAttack = true;
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
            GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_jump"], false);
        }
    }

    public override void NormalAttackExit()
    {
        player.isAttack = false;
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack1"], false);
        normalAttackCol.gameObject.SetActive(true);
    }

    public override void StrongAttackExit()
    {
        player.isAttack = false;
        OnlySingleton.Instance.camShake.ShakeCamera(7f, 0.1f);
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack2"], false);
        normalAttackCol_2.gameObject.SetActive(true);
    }
    public override float Right_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Green_Right"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Green_Right"].maxCoolTime;
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

    public override float R_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Green_R"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Green_R"].maxCoolTime;
    }

    public override float Change_Cool()
    {
        return base.Change_Cool();
    }
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
        NormalAttack();
    }

    public override void RightAction()
    {
        base.RightAction();
    }

    public override void Q_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Red_Q")) return;

        GameManager.Instance.coolTimeManager.GetCoolTime("Red_Q");

        Devastation();
    }

    public override void E_Action()
    {
        if (!GameManager.Instance.coolTimeManager.CoolCheck("Red_E")) return;

        GameManager.Instance.coolTimeManager.GetCoolTime("Red_E");
        HellDive();
    }

    private void HellDive()
    {
        player.Rigidbody.velocity = Vector3.zero;
        player.isAttack = true;
        player.transform.forward = Camera.main.transform.forward;
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        OnlySingleton.Instance.red_E_cam.Priority = 11;
        player.Rigidbody.useGravity = false;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[3];
        curKnockbackDir = knockbackDirs[3];

        player.currentCharacter.animator.Play(E_Anim.name, 3, 0f);
    }

    public override void R_Action()
    {

    }

    private void Devastation()
    {
        isActionSkill = true;
        player.Rigidbody.velocity = Vector3.zero;
        player.isAttack = true;
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


        if (Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
        {
            if (!animator.GetCurrentAnimatorStateInfo(3).IsName(followAnim.name))
            {
                player.currentCharacter.animator.Play(followAnim.name, 3, 0f);
                GameObject.Instantiate(Chasing_Particle.gameObject, model.transform.position + Vector3.up, model.transform.rotation);
            }


            player.transform.forward = target.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);

            RaycastHit hit;

            if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, Vector3.Distance(target.transform.position, player.transform.position), player.skillData.grappleMask))
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
        FollowAttack();
    }

    public override void FollowAttack()
    {
        player.isAttack = true;
        curParticle = normalAttackEffects[2];
        curKnockback = knockbacks[2];
        curKnockbackDir = knockbackDirs[2];
        player.currentCharacter.animator.Play(attackAnim[2].name, 3, 0f);
        followEnemy = false;
        comboCounter = 0;
    }

    public override void NormalAttackExit()
    {
        player.isAttack = false;
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack1"], false);
        normalAttackCol.gameObject.SetActive(true);
    }

    public override void StrongAttackExit()
    {
        player.isAttack = false;
        OnlySingleton.Instance.camShake.ShakeCamera(7f, 0.1f);
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack2"], false);
        normalAttackCol_2.gameObject.SetActive(true);
    }

    public override void Q_AnimExit()
    {
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack1"], false);
        GameObject temp = GameObject.Instantiate(Q_Particle.gameObject, model.transform.position, Quaternion.identity);
        temp.transform.forward = player.transform.forward;
        player.isAttack = false;
        Q_AttackCol.gameObject.SetActive(true);
    }

    public override void E_AnimExit()
    {
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack1"], false);
        player.isAttack = false;
        OnlySingleton.Instance.red_E_cam.Priority = 9;
        GameObject.Instantiate(E_Particle.gameObject, model.transform.position, Quaternion.identity);
        player.Rigidbody.useGravity = true;
        E_AttackCol.gameObject.SetActive(true);
    }



    public override void R_AnimExit()
    {

    }

    public override void AttackMotion()
    {
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length && !player.isAttack)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                player.isAttack = true;
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

                //player.Rigidbody.AddForce(player.transform.forward * 10f, ForceMode.VelocityChange);
            }
        }

    }

    public override void Sound(string name)
    {
        if (name == "jump")
        {
            GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_jump"], false);
        }
    }

    public override void NormalAttack()
    {
        if (followEnemy) return;

        if (player.targetSet.targetEnemy != null)
        {

            target = player.targetSet.targetEnemy;
            if (Vector3.Distance(target.transform.position, player.transform.position) > attackArea)
                followEnemy = true;
            else AttackMotion();
        }
        else
        {
            AttackMotion();
        }
    }

    public override float Right_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Red_Right"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Red_Right"].maxCoolTime;
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

    public override float R_Cool()
    {
        return GameManager.Instance.coolTimeManager.coolDic["Red_R"].curCoolTime /
            GameManager.Instance.coolTimeManager.coolDic["Red_R"].maxCoolTime;
    }

    public override float Change_Cool()
    {
        return base.Change_Cool();
    }
}

