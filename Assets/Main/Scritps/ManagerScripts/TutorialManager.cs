using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Ω∫≈≥ »πµÊ Bool")]
    public bool attack;
    public bool psyche;
    public bool graple;
    public bool characterChange;

    [Header("µ•¿Ã≈Õ")]
    [SerializeField] private Sprite[] skill_images;
    public int skillIndex;

    public void AttackSetBool(bool _bool)
    {
        attack = _bool;
        skillIndex++;
    }

    public void PsycheSetBool(bool _bool)
    {
        psyche = _bool;
        skillIndex++;
    }

    public void GrapleSetBool(bool _bool)
    {
        graple = _bool;
        skillIndex++;
    }

    public void CharacterChangeSetBool(bool _bool)
    {
        characterChange = _bool;
    }

    public void Tutorial_0_Event()
    {
        //GameManager.Instance.skill_Image.sprite = skill_images[0];
        GameManager.Instance.device_Image.GetComponent<DOTweenAnimation>().DORestartById("GetDevice");
    }

    public void Tutorial_1_Event()
    {
        PsycheSetBool(true);
        GameManager.Instance.skill_Image.sprite = skill_images[0];
        GameManager.Instance.skill_Image.GetComponent<DOTweenAnimation>().DORestartById("GetSkill");
    }
}
