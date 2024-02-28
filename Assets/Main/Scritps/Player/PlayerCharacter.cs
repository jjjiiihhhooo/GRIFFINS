using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerCharacter
{
    public Animator animator;
    public AnimationClip[] attackAnim;

    public Player player;

    public GameObject model;
    public GameObject weapon_obj;

    public int noOfClicks = 0;
    public int index;
    public int comboCounter;

    public float normalAttackDamage;

    public float lastClickedTime;
    public float lastComboEnd;

    public void Init(Player playerController)
    {
        player = playerController;
        weapon_obj.SetActive(false);
    }

    public virtual void Update()
    {
        ExitAttack();
    }
    public virtual void Catch()
    {
        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        if (player.targetSet.targetObject == null || player.skillData.isHand || player.skillData.handObj != null) return;
        if (player.currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle") || player.currentCharacter.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw")) return;
        GameManager.Instance.crossHair.SetActive(true);

        GameManager.Instance.staminaManager.staminaImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(140f, 35.4f, 0f);
        //GameManager.Instance.staminaManager.staminaImage.rectTransform.anchoredPosition = new Vector3(140f, 35.4f, 0f);

        player.currentCharacter.animator.SetBool("isHand", true);

        player.skillData.handObj = player.targetSet.targetObject;

        player.skillData.handObj.GetComponent<Collider>().isTrigger = true;


        Rigidbody rigid = player.skillData.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = player.skillData.camZoom;
        Outline outLine = player.skillData.handObj.GetComponent<Outline>();

        player.isPsyche = true;
        player.skillData.isHand = true;
        player.skillData.handObj.tag = "usingObject";
        rigid.useGravity = false;
        rigid.isKinematic = true;
        player.skillData.handObj.transform.DOKill(false);
        player.skillData.handObj.transform.DOMove(player.skillData.catchTransform.position, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => { player.skillData.handObj.transform.position = player.skillData.catchTransform.position; camZoom.maximumDistance = 2f; player.skillData.lookatTransform.localPosition = new Vector3(0.75f, 1.41f, 0f); player.skillData.handObj.transform.SetParent(player.transform); });
        player.movementStateMachine.ReusableData.ShouldWalk = true;
        player.movementStateMachine.ReusableData.ShouldSprint = false;
        if (player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.WalkingState);
        else player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
        outLine.DOKill(false);

        //outLine.OutlineWidth = 10f;
        player.CoroutineEvent(outLineCor(outLine, 10f, 1f, 10f));
    }

    public virtual void Throw()
    {
        if (!player.skillData.isHand || player.skillData.handObj == null) return;
        GameManager.Instance.crossHair.SetActive(false);
        GameManager.Instance.staminaManager.staminaImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(62f, 35.4f, 0f);
        //GameManager.Instance.staminaManager.staminaImage.rectTransform.anchoredPosition = new Vector3(62f, 35.4f, 0f);
        Rigidbody rigid = player.skillData.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = player.skillData.camZoom;
        Outline outLine = player.skillData.handObj.GetComponent<Outline>();

        player.isPsyche = false;

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
        player.movementStateMachine.ReusableData.ShouldWalk = false;
        if (player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.RunningState);

        player.skillData.isHand = false;
        player.Rigidbody.velocity = Vector3.zero;
        player.skillData.handObj = null;
    }

    public virtual void StartGrapple()
    {
        if (player.skillData.grapplingCdTimer > 0) return;
        player.skillData.grappling = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, player.skillData.maxGrappleDistance, player.skillData.grappleMask))
        {
            player.skillData.grapplePoint = hit.point;
            if (!player.isGround) player.CoroutineEvent(DelayCor(player.skillData.grappleDelayTime, 0));

        }
        else
        {
            player.skillData.grapplePoint = Camera.main.transform.position + Camera.main.transform.forward * player.skillData.maxGrappleDistance;
            player.CoroutineEvent(DelayCor(player.skillData.grappleDelayTime, 1));
        }

        player.skillData.lr.enabled = true;
        player.skillData.lr.SetPosition(1, player.skillData.grapplePoint);
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
        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);


        player.currentCharacter.animator.SetBool("isGrappling", true);
        player.transform.rotation = player.CameraRecenteringUtility.VirtualCamera.transform.rotation;
        player.transform.rotation = new Quaternion(0f, player.CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, player.transform.rotation.w);

        Vector3 lowestPoint = new Vector3(player.transform.position.x, player.transform.position.y - 1f, player.transform.position.z);



        float grapplePointRelativeYPos = player.skillData.grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + player.skillData.overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = player.skillData.overshootYAxis;

        player.skillData.velocity = CalculateJumpVelocity(player.transform.position, player.skillData.grapplePoint, highestPointOnArc);
        player.ResizableCapsuleCollider.SlopeData.StepHeightPercentage = 0f;
        player.skillData.touch = true;
        player.GetComponent<Rigidbody>().velocity = player.skillData.velocity;
    }

    public virtual void StopGrapple()
    {
        player.currentCharacter.animator.SetBool("isGrappling", false);
        player.ResizableCapsuleCollider.SlopeData.StepHeightPercentage = 0.25f;
        player.skillData.grappling = false;
        player.skillData.grapplingCdTimer = player.skillData.grapplingCd;

        player.skillData.lr.enabled = false;
    }

    public virtual void GCatch()
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

    public virtual void GPull()
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

    public virtual void NormalAttack()
    {
        
        if (Time.time - lastComboEnd > 0.1f && comboCounter < attackAnim.Length)
        {
            player.CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.4f)
            {
                weapon_obj.SetActive(true);
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

        if(player.targetSet.targetEnemy != null)
        {
            player.transform.forward = player.targetSet.targetEnemy.transform.position - player.transform.position;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f); 
        }


    }

    public virtual void Interaction()
    {
        player.targetSet.targetInteraction?.OnInteract();
        
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

    public override void Update()
    {
        base.Update();
    }
    public override void Catch()
    {
        base.Catch();
    }

    public override void Throw()
    {
        base.Throw();
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

    public override void GCatch()
    {
        base.GCatch();
    }

    public override void GPull()
    {
        base.GPull();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void Interaction()
    {
        base.Interaction();
    }

    public override Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        return base.CalculateJumpVelocity(startPoint, endPoint, trajectoryHeight);
    }
}

[System.Serializable]
public class GreenCharacter : PlayerCharacter
{
    public override void Update()
    {
        base.Update();
    }
    public override void Catch()
    {
        base.Catch();
    }

    public override void Throw()
    {
        base.Throw();
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

    public override void GCatch()
    {
        base.GCatch();
    }

    public override void GPull()
    {
        base.GPull();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }


    public override void Interaction()
    {
        base.Interaction();
    }
    public override Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        return base.CalculateJumpVelocity(startPoint, endPoint, trajectoryHeight);
    }
}

[System.Serializable]
public class BlueCharacter : PlayerCharacter
{
    public override void Update()
    {
        base.Update();
    }
    public override void Catch()
    {
        base.Catch();
    }

    public override void Throw()
    {
        base.Throw();
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

    public override void GCatch()
    {
        base.GCatch();
    }

    public override void GPull()
    {
        base.GPull();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void Interaction()
    {
        base.Interaction();
    }

    public override Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        return base.CalculateJumpVelocity(startPoint, endPoint, trajectoryHeight);
    }
}

[System.Serializable]
public class RedCharacter : PlayerCharacter
{
    public override void Update()
    {
        base.Update();
    }
    public override void Catch()
    {
        base.Catch();
    }

    public override void Throw()
    {
        base.Throw();
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

    public override void GCatch()
    {
        base.GCatch();
    }

    public override void GPull()
    {
        base.GPull();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void Interaction()
    {
        base.Interaction();
    }

    public override Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        return base.CalculateJumpVelocity(startPoint, endPoint, trajectoryHeight);
    }
}

