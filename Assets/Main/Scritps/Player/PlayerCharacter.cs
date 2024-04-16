using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerCharacter
{
    public Animator animator;
    
    public Player player;

    public GameObject model;

    public bool isGrappleReady;
    public int index;

    public int comboCounter;

    public float normalAttackDamage;

    public float lastClickedTime;
    public float lastComboEnd;

    [Header("Collider")]
    public AttackCol normalAttackCol;
    public AttackCol normalAttackCol_2;

    //[Header("Effect")]
    //public ParticleSystem normalAttackEffect;
    //public ParticleSystem normalAttackEffect_2;


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

    public void ExitAttack()
    {
        if(player.currentCharacter.animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.9f && player.currentCharacter.animator.GetCurrentAnimatorStateInfo(3).IsTag("Attack"))
        {
            player.isAttack = false;
            player.Invoke("EndCombo", 0.5f);
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
    public override void CharacterChange()
    {
        PutDown();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LeftAction()
    {
        
    }

    public override void RightAction()
    {
        if (player.skillData.isHand) Throw();
        else Catch();
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
        
        if(save)
        {
            GameObject.Destroy(player.skillData.handObj);
        }

        player.skillData.handObj = null;
    }

    private void Catch(bool load = false)
    {
        if(!load)
        {
            if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
            if (player.targetSet.targetObject == null || player.skillData.isHand || player.skillData.handObj != null || !player.isGround) return;
        }
        
        if (player.currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") || player.currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw")) return;
        GameManager.Instance.uiManager.crossHair.SetActive(true);

        GameManager.Instance.staminaManager.staminaImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(140f, 35.4f, 0f);
        //GameManager.Instance.staminaManager.staminaImage.rectTransform.anchoredPosition = new Vector3(140f, 35.4f, 0f);

        player.currentCharacter.animator.SetBool("isHand", true);

        if(!load)
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
        
        if(!load)
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

}

[System.Serializable]
public class GreenCharacter : PlayerCharacter
{
    public override void CharacterChange()
    {
        GameManager.Instance.predictionHitTransform.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !player.isGround)
        {
            player.currentCharacter.StopGrapple();
            player.swinging.StopSwing();
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.isGround && player.skillData.grappling)
        {
            ExecuteGrapple();
        }
    }

    public override void LeftAction()
    {
        if (!player.isGround) StartGrappleAnim();
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

    public override void Q_Action()
    {

    }

    public override void E_Action()
    {

    }

    public override void R_Action()
    {

    }

    public override void ExecuteGrapple()
    {
        base.ExecuteGrapple();
    }

    public override void StopGrapple()
    {
        base.StopGrapple();
    }

}

[System.Serializable]
public class RedCharacter : PlayerCharacter
{
    public AnimationClip[] attackAnim;


    public override void CharacterChange()
    {

    }

    public override void Update()
    {
        ExitAttack();
    }

    public override void LeftAction()
    {
        if (player.Animator.GetBool("isDashing")) return;
        NormalAttack();
    }

    public override void RightAction()
    {
        base.RightAction();
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

    private void NormalAttack()
    {
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                player.isAttack = true;
                player.currentCharacter.animator.Play(attackAnim[comboCounter].name, 3, 0f);
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter >= attackAnim.Length)
                {
                    comboCounter = 0;
                }
            }
        }

        player.Rigidbody.velocity = Vector3.zero;

        if (player.targetSet.targetEnemy != null)
        {
            player.transform.forward = player.targetSet.targetEnemy.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        }
    }

}

