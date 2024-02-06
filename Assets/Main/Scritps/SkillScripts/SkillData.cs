using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine.InputSystem.EnhancedTouch;

public class SkillData : MonoBehaviour
{

    public string[] skillName = { "Catch" //0 Q
                                , "Throw" //1 좌클릭
                                , "StartGrapple"
                                , "GCatch"
                                , "GPull"
    };
    public int skillIndex;
    public bool isHand; //잡았는지
    public bool isGHand;
    private Player player;

    

    private void Catch() //잡는거
    {
        if (player == null) player = Player.Instance;

        if (player.targetSet.targetGameObject == null || isHand || player.skillFunction.handObj != null) return;
        if (player.Animator.GetCurrentAnimatorStateInfo(1).IsName("White_Idle") || player.Animator.GetCurrentAnimatorStateInfo(1).IsName("White_Throw")) return;
        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        

        player.Animator.SetBool("isHand", true);
        SkillFunction skill = player.skillFunction;
        skill.handObj = player.targetSet.targetGameObject;

        skill.handObj.GetComponent<Collider>().isTrigger = true;


        Rigidbody rigid = skill.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = FindObjectOfType<CameraZoom>();    
        Outline outLine = skill.handObj.GetComponent<Outline>();
        
        isHand = true;
        skill.handObj.tag = "usingObject";
        rigid.useGravity = false;
        rigid.isKinematic = true;
        skill.handObj.transform.DOKill(false);
        skill.handObj.transform.DOMove(skill.catchTransform.position, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => { skill.handObj.transform.position = skill.catchTransform.position; camZoom.maximumDistance = 2f; skill.lookatTransform.localPosition = new Vector3(0.75f, 1.41f, 0f); skill.handObj.transform.SetParent(transform); });
        player.movementStateMachine.ReusableData.ShouldWalk = true;
        player.movementStateMachine.ReusableData.ShouldSprint = false;
        if (player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.WalkingState);
        else player.movementStateMachine.ChangeState(player.movementStateMachine.IdlingState);
        outLine.DOKill(false);

        //outLine.OutlineWidth = 10f;
        StartCoroutine(outLineCor(outLine, 10f, 1f, 10f));
        //DOTween.To(() => outLine.OutlineWidth, x => outLine.OutlineWidth = x, 10f,0.3f);
        
    }

    private IEnumerator outLineCor(Outline outLine, float value, float oper, float pitch)
    {
        if(oper > 0f)
        {
            while (outLine.OutlineWidth < value)
            {
                outLine.OutlineWidth += Time.deltaTime * oper * pitch;
                yield return new WaitForEndOfFrame();
                if (!isHand) break;
            }
        }
        else if (oper < 0f)
        {
            while (outLine.OutlineWidth > value)
            {
                outLine.OutlineWidth += Time.deltaTime * oper * pitch;
                yield return new WaitForEndOfFrame();
            }
            outLine.tag = "useObject";
        }
    }

    private void Throw() //던지는거
    {
        if (!isHand || player.skillFunction.handObj == null) return;
        SkillFunction skill = player.skillFunction;
        

        Rigidbody rigid = skill.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = FindObjectOfType<CameraZoom>();
        Outline outLine = skill.handObj.GetComponent<Outline>();

        if (skill.handObj.gameObject.name == "Stamina_obj")
        {
            player.Animator.SetBool("isHand", false);
            GameManager.Instance.staminaManager.PlusStamina(30f);
            Destroy(skill.handObj.gameObject);
        }
        else
        {
            player.Animator.SetTrigger("isThrow");
            player.Animator.SetBool("isHand", false);
            GameManager.Instance.staminaManager.MinusStamina(20f);
            rigid.useGravity = true;
            skill.handObj.transform.SetParent(null);
            rigid.isKinematic = false;
            rigid.AddForce(Camera.main.transform.forward * 30f, ForceMode.Impulse);
            StartCoroutine(outLineCor(outLine, 0f, -1f, 10f));
        }

        skill.handObj.GetComponent<BoxCollider>().isTrigger = false;

        camZoom.minimumDistance = 6f;
        camZoom.maximumDistance = 6f;
        camZoom.transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(()=> camZoom.minimumDistance = 1f);
        
        player.skillFunction.lookatTransform.localPosition = new Vector3(0, 1.23f, 0f);
        player.movementStateMachine.ReusableData.ShouldWalk = false;
        if(player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.RunningState);

        
        //DOTween.To(() => outLine.OutlineWidth, x => outLine.OutlineWidth = x, 0f, 0.3f);
        //outLine.OutlineWidth = 0f;
        isHand = false;
        player.Rigidbody.velocity = Vector3.zero; 
        skill.handObj = null;
        
    }

    private void StartGrapple()
    {
        if (player == null) player = Player.Instance;
        SkillFunction skill = player.skillFunction;

        if (skill.grapplingCdTimer > 0) return;
        skill.grappling = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, skill.maxGrappleDistance, skill.grappleMask))
        {
            skill.grapplePoint = hit.point;
            if(!player.isGround) Invoke(nameof(ExecuteGrapple), skill.grappleDelayTime);

        }
        else
        {
            skill.grapplePoint = Camera.main.transform.position + Camera.main.transform.forward * skill.maxGrappleDistance;
            Invoke(nameof(StopGrapple), skill.grappleDelayTime);
        }

        skill.lr.enabled = true;
        skill.lr.SetPosition(1, skill.grapplePoint);
    }

    public void ExecuteGrapple()
    {

        if (player == null) player = Player.Instance;
        SkillFunction skill = player.skillFunction;

        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);


        player.Animator.SetBool("isGrappling", true);
        transform.rotation = player.CameraRecenteringUtility.VirtualCamera.transform.rotation;
        transform.rotation = new Quaternion(0f, player.CameraRecenteringUtility.VirtualCamera.transform.rotation.y, 0f, transform.rotation.w);

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        

        float grapplePointRelativeYPos = skill.grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + skill.overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = skill.overshootYAxis;

        skill.velocity = CalculateJumpVelocity(transform.position, skill.grapplePoint, highestPointOnArc);
        player.ResizableCapsuleCollider.SlopeData.StepHeightPercentage = 0f;
        skill.touch = true;
        GetComponent<Rigidbody>().velocity = skill.velocity;
        
    }


    public void StopGrapple()
    {
        if (player == null) player = Player.Instance;
        SkillFunction skill = player.skillFunction;
        player.Animator.SetBool("isGrappling", false);
        player.ResizableCapsuleCollider.SlopeData.StepHeightPercentage = 0.25f;
        skill.grappling = false;
        skill.grapplingCdTimer = skill.grapplingCd;

        skill.lr.enabled = false;
    }

    public void GCatch()
    {
        if (player == null) player = Player.Instance;
        SkillFunction skill = player.skillFunction;
        if (player.targetSet.grappleTargetObject == null || isGHand || player.skillFunction.GhandObj != null) return;
        if (skill.grapplingCdTimer > 0) return;

        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        GameManager.Instance.staminaManager.MinusStamina(20f);


        skill.GhandObj = player.targetSet.grappleTargetObject;

        skill.Gjoint = skill.GhandObj.gameObject.AddComponent<SpringJoint>();
        skill.Gjoint.autoConfigureConnectedAnchor = false;
        //skill.Gjoint.connectedAnchor = transform.position;

        skill.distanceFromPoint = Vector3.Distance(transform.position, player.skillFunction.GhandObj.transform.position);

        // the distance grapple will try to keep from grapple point. 
        skill.Gjoint.maxDistance = skill.distanceFromPoint * 0.8f;
        skill.Gjoint.minDistance = skill.distanceFromPoint * 0.25f;

        // customize values as you like
        skill.Gjoint.spring = 4.5f;
        skill.Gjoint.damper = 7f;
        skill.Gjoint.massScale = 4.5f;

        isGHand = true;

        skill.Glr.enabled = true;
    }

    public void GPull()
    {
        if (player == null) player = Player.Instance;
        SkillFunction skill = player.skillFunction;
        Destroy(skill.Gjoint);

        Vector3 lowestPoint = new Vector3(skill.GhandObj.transform.position.x, skill.GhandObj.transform.position.y - 1f, skill.GhandObj.transform.position.z);


        float grapplePointRelativeYPos = transform.position.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + skill.overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = skill.overshootYAxis;

        skill.Gvelocity = CalculateJumpVelocity(skill.GhandObj.transform.position, transform.position, highestPointOnArc);
        isGHand = false;
        skill.GhandObj.transform.GetComponent<Rigidbody>().velocity = skill.Gvelocity;
        skill.Glr.enabled = false;
        skill.GhandObj = null;
    }


















    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight) //목표 위치까지 포물선 trajectoryHeight 높이 추가
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
