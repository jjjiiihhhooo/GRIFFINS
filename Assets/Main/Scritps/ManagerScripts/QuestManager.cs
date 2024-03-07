using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        QuestText();
    }

    private void QuestText()
    {
        int count = 0;

        while (curQuest != null && count < curQuest.QuestDatas.Length)
        {
            int temp = count + 1;
            if (curQuest.QuestDatas[count].type == QuestType.Chat || curQuest.QuestDatas[count].type == QuestType.Interaction || curQuest.QuestDatas[count].type == QuestType.Position)
                questTexts[count].text = temp.ToString() + ". " + curQuest.QuestDatas[count].questString;
            else if (curQuest.QuestDatas[count].type == QuestType.Enemy)
                questTexts[count].text = temp.ToString() + ". " + curQuest.QuestDatas[count].questString + "(" + curQuest.QuestDatas[count].enemyCurCount.ToString() + " / " + curQuest.QuestDatas[count].enemyCompleteCount.ToString() + ")";
            else if (curQuest.QuestDatas[count].type == QuestType.Item)
                questTexts[count].text = temp.ToString() + ". " + curQuest.QuestDatas[count].questString + "(" + curQuest.QuestDatas[count].itemCurCount.ToString() + " / " + curQuest.QuestDatas[count].itemCompleteCount.ToString() + ")";

            questTexts[count].gameObject.SetActive(true);
            count++;
        }

    }

    public void ChatQuestCheck(string chatKey)
    {
        if (curQuest == null) return;

        int count = 0;

        while(curQuest != null && count < curQuest.QuestDatas.Length)
        {
            if (curQuest.QuestDatas[count].type == QuestType.Chat)
            {
                if (!curQuest.QuestDatas[count].clear && chatKey == curQuest.QuestDatas[count].chatKey)
                {
                    QuestClear(curQuest.QuestDatas[count], count);
                }
            }
            count++;
        }

    }
    
    public void ItemQuestcheck(string name)
    {
        if (curQuest == null) return;

        for (int i = 0; i < curQuest.QuestDatas.Length; i++)
        {
            if (curQuest.QuestDatas[i].type == QuestType.Item)
            {
                if (!curQuest.QuestDatas[i].clear && name == curQuest.QuestDatas[i].itemName)
                {
                    curQuest.QuestDatas[i].itemCurCount++;

                    if (curQuest.QuestDatas[i].itemCurCount >= curQuest.QuestDatas[i].itemCompleteCount)
                    {
                        QuestClear(curQuest.QuestDatas[i], i);
                    }
                }
            }
        }
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

        int count = 0;

        while (curQuest != null && count < curQuest.QuestDatas.Length)
        {
            if (curQuest.QuestDatas[count].type == QuestType.Enemy)
            {
                if (!curQuest.QuestDatas[count].clear && name == curQuest.QuestDatas[count].enemyName)
                {
                    curQuest.QuestDatas[count].enemyCurCount++;

                    if (curQuest.QuestDatas[count].enemyCurCount >= curQuest.QuestDatas[count].enemyCompleteCount)
                    {
                        QuestClear(curQuest.QuestDatas[count], count);
                    }
                }
            }
            QuestText();
            count++;
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


