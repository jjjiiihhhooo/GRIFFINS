using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoolTimeManager : MonoBehaviour
{
    public class CoolData
    {
        public CoolData(int max, int cur, int index)
        {
            this.maxCoolTime = max;
            this.curCoolTime = cur;
            this.index = index;
        }

        public float maxCoolTime;
        public float curCoolTime;
        public int index;
    }

    public Dictionary<string, CoolData> coolDic;


    public void Init()
    {
        coolDic = new Dictionary<string, CoolData>();
        coolDic.Add("CharacterChange", new CoolData(2, 0, -1));
        coolDic.Add("White_Right", new CoolData(5, 0, 0));
        coolDic.Add("White_Q", new CoolData(5, 0, 0));
        coolDic.Add("White_E", new CoolData(5, 0, 1));
        coolDic.Add("White_R", new CoolData(5, 0, 0));
        coolDic.Add("Green_Right", new CoolData(5, 0, 0));
        coolDic.Add("Green_Q", new CoolData(5, 0, 2));
        coolDic.Add("Green_E", new CoolData(5, 0, 3));
        coolDic.Add("Green_R", new CoolData(5, 0, 0));
        coolDic.Add("Red_Right", new CoolData(5, 0, 0));
        coolDic.Add("Red_Q", new CoolData(5, 0, 4));
        coolDic.Add("Red_E", new CoolData(1, 0, 5));
        coolDic.Add("Red_R", new CoolData(1, 0, 0));

    }

    private IEnumerator CoolDownCor(string name)
    {
        while (coolDic[name].curCoolTime > 0)
        {
            coolDic[name].curCoolTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (name == "CharacterChange")
        {
            GameManager.Instance.uiManager.ChangeAnim();
        }

        coolDic[name].curCoolTime = 0;

        Player.Instance.currentCharacter.SkillCoolTimeResetAnim(coolDic[name].index);
    }

    public void SetCoolTime(string name, float num)
    {
        coolDic[name].curCoolTime = num;
    }

    public void GetCoolTime(string name)
    {
        coolDic[name].curCoolTime = coolDic[name].maxCoolTime;
        StopCoroutine(CoolDownCor(name));
        StartCoroutine(CoolDownCor(name));
    }

    public bool CoolCheck(string name)
    {
        if (coolDic[name].curCoolTime > 0) return false;
        else return true;
    }

}
