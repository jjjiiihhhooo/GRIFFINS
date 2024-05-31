using UnityEngine;

public class QuestContainer : MonoBehaviour
{

    public QuestBox curQuest;

    public void SetQuest()
    {
        GameManager.Instance.questManager.QuestInput(curQuest);
    }

    public void CurWaveDestroy()
    {
        GameManager.Instance.enemyManager.curWaveObject.transform.GetComponentInChildren<BattleArea>().DestroyAnim();
    }
}
