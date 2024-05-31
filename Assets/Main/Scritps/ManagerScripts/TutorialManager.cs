using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("��ų ȹ�� Bool")]
    public bool attack;
    public bool psyche;
    public bool graple;
    public bool characterChange;

    [Header("������")]
    public Sprite[] skill_images;
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

}
