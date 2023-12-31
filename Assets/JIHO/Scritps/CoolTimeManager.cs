using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoolTimeManager : MonoBehaviour
{
    public static CoolTimeManager instance;

    public class CoolData
    {
        public CoolData(int max, int cur)
        {
            this.maxCoolTime = max;
            this.curCoolTime = cur;
        }

        public float maxCoolTime;
        public float curCoolTime;
    }

    public Dictionary<string, CoolData> coolDic;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        coolDic = new Dictionary<string, CoolData>();
        coolDic.Add("Dash", new CoolData(2, 0));
        coolDic.Add("DownStream", new CoolData(2, 0));
        coolDic.Add("CharacterChange", new CoolData(1, 0));
        coolDic.Add("Tornado", new CoolData(5, 0));
    }

    private IEnumerator CoolDownCor(string name)
    {
        while (coolDic[name].curCoolTime > 0)
        {
            coolDic[name].curCoolTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        coolDic[name].curCoolTime = 0;
    }

    public void SetCoolTime(string name, float num)
    {
        coolDic[name].curCoolTime = num;
    }

    public void GetCoolTime(string name)
    {
        coolDic[name].curCoolTime = coolDic[name].maxCoolTime;
        StartCoroutine(CoolDownCor(name));
    }

    public bool CoolCheck(string name)
    {
        if (coolDic[name].curCoolTime > 0) return false;
        else return true;
    }

}
