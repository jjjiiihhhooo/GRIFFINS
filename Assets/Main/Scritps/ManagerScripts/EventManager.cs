using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{


    public void Tutorial_0_Event()
    {
        //GameManager.Instance.skill_Image.sprite = skill_images[0];
        GameManager.Instance.questManager.QuestInput("Tutorial_0_1");
        GameManager.Instance.device_Image.GetComponent<DOTweenAnimation>().DORestartById("GetDevice");
        GameManager.Instance.questManager.questImage.GetComponent<DOTweenAnimation>().DORestartById("GetQuestBoard");

    }

    public void Tutorial_2_Event()
    {
        GameManager.Instance.tutorialManager.PsycheSetBool(true);
        GameManager.Instance.skill_Image.sprite = GameManager.Instance.tutorialManager.skill_images[0];
        GameManager.Instance.skill_Image.GetComponent<DOTweenAnimation>().DORestartById("GetSkill");
    }


    public void Tutorial_2_Color(string name)
    {
        GameManager.Instance.questManager.PositionQuestCheck(name);
    }

    public void Tutorial_2_Interaction()
    {
        FindAnyObjectByType<InteractableObject>().SetReady(true);
    }

    public void Tutorial_3_Event()
    {
        GameManager.Instance.tutorialManager.AttackSetBool(true);
    }

    public void Tutorial_3_Floor()
    {
        GameObject.Find("Floor").GetComponent<ObjectAction>().AnimationAction("Tutorial_3_Floor");
    }
}
