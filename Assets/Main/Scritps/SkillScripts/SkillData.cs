using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sirenix.Serialization;

public class SkillData : MonoBehaviour
{

    public string[] skillName = { "Catch" //0 Q
                                , "Throw" //1 좌클릭
    };
    public int skillIndex;
    public bool isHand; //잡았는지
    private Player player;


    private void Catch() //잡는거
    {
        if (player == null) player = Player.Instance;

        if (player.targetSet.targetGameObject == null || isHand || player.skillFunction.handObj != null) return;
        if (player.Animator.GetCurrentAnimatorStateInfo(1).IsName("White_Idle") || player.Animator.GetCurrentAnimatorStateInfo(1).IsName("White_Throw")) return;
        if (!GameManager.Instance.staminaManager.ChechStamina(20f)) return;
        
        GameManager.Instance.staminaManager.MinusStamina(20f);
        player.Animator.SetBool("isHand", true);
        SkillFunction skill = player.skillFunction;
        skill.handObj = player.targetSet.targetGameObject;

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
        Debug.Log("잡다");
        
    }

    private IEnumerator outLineCor(Outline outLine, float value, float oper, float pitch)
    {
        if(oper > 0f)
        {
            while (outLine.OutlineWidth < value)
            {
                outLine.OutlineWidth += Time.deltaTime * oper * pitch;
                yield return new WaitForEndOfFrame();
                Debug.Log("[Plus]value : " + value + "current : " + outLine.OutlineWidth);
                if (!isHand) break;
            }
        }
        else if (oper < 0f)
        {
            while (outLine.OutlineWidth > value)
            {
                outLine.OutlineWidth += Time.deltaTime * oper * pitch;
                Debug.Log("[Minus]value : " + value + "current : " + outLine.OutlineWidth);
                yield return new WaitForEndOfFrame();
            }
            outLine.tag = "useObject";
        }
    }

    private void Throw() //던지는거
    {
        if (!isHand || player.skillFunction.handObj == null) return;
        SkillFunction skill = player.skillFunction;
        player.Animator.SetBool("isHand", false); //나중에 던지는 애니메이션으로 바꿀 생각
        Rigidbody rigid = skill.handObj.GetComponent<Rigidbody>();
        CameraZoom camZoom = FindObjectOfType<CameraZoom>();
        Outline outLine = skill.handObj.GetComponent<Outline>();

        
        rigid.useGravity = true;
        skill.handObj.transform.SetParent(null);
        rigid.isKinematic = false;
        rigid.AddForce(Camera.main.transform.forward * 30f,ForceMode.Impulse);
        camZoom.minimumDistance = 6f;
        camZoom.maximumDistance = 6f;
        camZoom.transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(()=> camZoom.minimumDistance = 1f);
        
        player.skillFunction.lookatTransform.localPosition = new Vector3(0, 1.23f, 0f);
        player.movementStateMachine.ReusableData.ShouldWalk = false;
        if(player.movementStateMachine.ReusableData.MovementInput != Vector2.zero) player.movementStateMachine.ChangeState(player.movementStateMachine.RunningState);

        StartCoroutine(outLineCor(outLine, 0f, -1f, 10f));

        //DOTween.To(() => outLine.OutlineWidth, x => outLine.OutlineWidth = x, 0f, 0.3f);
        //outLine.OutlineWidth = 0f;
        isHand = false;
        player.Rigidbody.velocity = Vector3.zero; 
        skill.handObj = null;
        Debug.Log("던지다");
        
    }
}
