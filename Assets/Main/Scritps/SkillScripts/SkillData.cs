using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using genshin;

public class SkillData : MonoBehaviour
{

    public string[] skillName = { "Catch" //0 Q
                                , "Throw" //1 좌클릭
    };
    public int skillIndex;

    public bool isHand; //잡았는지
    public GameObject aimingCam;
    

    private void Catch() //잡는거
    {
        if (Player.Instance.targetSet.targetGameObject == null || isHand) return;
        
        isHand = true;
        GameObject target;
        SkillFunction skill = Player.Instance.skillFunction;
        target = Player.Instance.targetSet.targetGameObject;
        target.transform.GetComponent<Rigidbody>().useGravity = false;
        target.transform.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.DOMove(skill.catchTransform.position, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => { target.transform.position = skill.catchTransform.position; FindObjectOfType<CameraZoom>().maximumDistance = 2f; Player.Instance.skillFunction.lookatTransform.localPosition = new Vector3(0.75f, 1.41f, 0f); target.transform.SetParent(transform); });
        
        Player.Instance.movementStateMachine.ReusableData.ShouldWalk = true;
        DOTween.To(() => target.GetComponent<Outline>().OutlineWidth, x => target.GetComponent<Outline>().OutlineWidth = x, 4.2f,3f);
        Debug.Log("잡다");
        
    }


    private void Throw() //던지는거
    {
        if (!isHand) return;
        isHand = false;
        GameObject target;
        target = Player.Instance.targetSet.targetGameObject;
        target.transform.GetComponent<Rigidbody>().useGravity = true;
        target.transform.SetParent(null);
        target.transform.GetComponent<Rigidbody>().isKinematic = false;
        target.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 30f,ForceMode.Impulse);
        FindObjectOfType<CameraZoom>().minimumDistance = 6f;
        FindObjectOfType<CameraZoom>().maximumDistance = 6f;
        FindObjectOfType<CameraZoom>().transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(()=> FindObjectOfType<CameraZoom>().minimumDistance = 1f);
        
        Player.Instance.skillFunction.lookatTransform.localPosition = new Vector3(0, 1.23f, 0f);
        Player.Instance.movementStateMachine.ReusableData.ShouldWalk = false;
        DOTween.To(() => target.GetComponent<Outline>().OutlineWidth, x => target.GetComponent<Outline>().OutlineWidth = x, 0f, 0.5f);
        Debug.Log("던지다");
        
    }
}
