using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoolTimeManager : MonoBehaviour
{
    public static CoolTimeManager Instance;

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
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
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
        coolDic.Add("SuperJump", new CoolData(2, 0));
    }

    private IEnumerator CoolDownCor(string name)
    {
        while (coolDic[name].curCoolTime > 0)
        {
            coolDic[name].curCoolTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
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
