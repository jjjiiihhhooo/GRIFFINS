using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestManager : SerializedMonoBehaviour
{
    public Dictionary<string, QuestBox> questDictionary;

    public QuestBox curQuest;

    public Image questImage;
    public TextMeshProUGUI[] questTexts;

 

    private void QuestDestroy()
    {
        curQuest = null;
    }

    private void QuestClear(QuestData questData, int index)
    {
        questData.clear = true;
        questTexts[index].text = questTexts[index].text + " (완료)";
        if (questData.clear_event != null) questData.clear_event.Invoke();

        QuestAllClear();
    }

    private void QuestAllClear()
    {
        if (curQuest.QuestDatas.Length <= 0) return;

        for(int i = 0; i < curQuest.QuestDatas.Length; i++)
        {
            if (!curQuest.QuestDatas[i].clear) return;
        }

        if (curQuest.clear_event != null) curQuest.clear_event.Invoke();

        GameManager.Instance.guideManager.SetMessage("퀘스트 클리어");
        QuestTextFalse();
        QuestDestroy();
    }

    private void QuestTextFalse()
    {
        for(int i = 0; i < questTexts.Length; i++)
        {
            questTexts[i].gameObject.SetActive(false);
        }
    }

    public void QuestInput(string key)
    {
        curQuest = questDictionary[key];

        for(int i = 0; i < curQuest.QuestDatas.Length; i++)
        {
            int temp = i + 1;
            if (curQuest.QuestDatas[i].type == QuestType.Chat || curQuest.QuestDatas[i].type == QuestType.Interaction || curQuest.QuestDatas[i].type == QuestType.Position)
                questTexts[i].text = temp.ToString() + ". " + curQuest.QuestDatas[i].questString;
            else if(curQuest.QuestDatas[i].type == QuestType.Enemy)
                questTexts[i].text = temp.ToString() + ". " + curQuest.QuestDatas[i].questString + "(" + curQuest.QuestDatas[i].enemyCurCount.ToString() + " / " + curQuest.QuestDatas[i].enemyCompleteCount.ToString() + ")";
            else if(curQuest.QuestDatas[i].type == QuestType.Item)
                questTexts[i].text = temp.ToString() + ". " + curQuest.QuestDatas[i].questString + "(" + curQuest.QuestDatas[i].itemCurCount.ToString() + " / " + curQuest.QuestDatas[i].itemCompleteCount.ToString() + ")";
            
            questTexts[i].gameObject.SetActive(true);
        }
    }

    public void ChatQuestCheck(string chatKey)
    {
        if (curQuest == null) return;

        for(int i = 0; i < curQuest.QuestDatas.Length; i++)
        {
            if (curQuest.QuestDatas[i].type == QuestType.Chat)
            {
                if (!curQuest.QuestDatas[i].clear && chatKey == curQuest.QuestDatas[i].chatKey)
                {
                    QuestClear(curQuest.QuestDatas[i], i);
                }
            }
        }
    }
    
    public void ItemQuestcheck()
    {

    }

    public void InteractionQuestCheck(string name)
    {
        if (curQuest == null) return;

        int count = 0;

        while(curQuest != null && count < curQuest.QuestDatas.Length)
        {
            if (curQuest.QuestDatas[count].type == QuestType.Interaction)
            {
                if (!curQuest.QuestDatas[count].clear && name == curQuest.QuestDatas[count].interactionName)
                {
                    QuestClear(curQuest.QuestDatas[count], count);
                }
            }
            count++;
        }
    }
    
    public void EnemyQuestCheck(string name)
    {
        if (curQuest == null) return;

        for(int i = 0; i < curQuest.QuestDatas.Length; i++)
        {
            if (curQuest.QuestDatas[i].type == QuestType.Enemy)
            {
                if (!curQuest.QuestDatas[i].clear && name == curQuest.QuestDatas[i].enemyName)
                {
                    curQuest.QuestDatas[i].enemyCurCount++;

                    if (curQuest.QuestDatas[i].enemyCurCount >= curQuest.QuestDatas[i].enemyCompleteCount)
                    {
                        QuestClear(curQuest.QuestDatas[i], i);
                    }
                }
            }
        }
    }

    public void PositionQuestCheck(string name)
    {
        if (curQuest == null) return;

        int count = 0;

        while(curQuest != null && count < curQuest.QuestDatas.Length)
        {
            if (curQuest.QuestDatas[count].type == QuestType.Position)
            {
                if (!curQuest.QuestDatas[count].clear && name == curQuest.QuestDatas[count].posName)
                {
                    QuestClear(curQuest.QuestDatas[count], count);
                }
            }
            count++;
        }

    }
    
}


