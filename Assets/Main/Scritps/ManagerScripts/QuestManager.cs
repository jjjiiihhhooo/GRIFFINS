using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class QuestManager : SerializedMonoBehaviour
{
    public Dictionary<string, QuestBox> questDictionary;

    public QuestBox curQuest;

    public DOTweenAnimation titleLogo;
    public TextMeshProUGUI titleText;

    public QuestUIData[] questUIdatas;



    public void UpdateQuest()
    {
        if (curQuest == null) 
        {
            curQuest = FindObjectOfType<CurrentQuest>().curQuest;
            titleText.text = FindObjectOfType<CurrentQuest>().QuestTitleText;
            titleLogo.DORestartById("Start");
        }
    }

    public void QuestDestroy()
    {
        //UpdateQuest();
        curQuest = null;
    }

    private void QuestClear(QuestData questData, int index)
    {
        //UpdateQuest();
        questData.clear = true;
        questUIdatas[index].QuestClear();
        //QuestText();
        //questTexts[index].text = questTexts[index].text + " (완료)";

        if (questData.clear_event != null) questData.clear_event.Invoke();

        QuestAllClear();
    }

    private void QuestAllClear()
    {
        //UpdateQuest();
        if (curQuest.QuestDatas.Length <= 0) return;
        for (int i = 0; i < curQuest.QuestDatas.Length; i++)
        {
            if (!curQuest.QuestDatas[i].clear) return;
        }

        if (curQuest.clear_event != null) curQuest.clear_event.Invoke();

        GameManager.Instance.guideManager.SetMessage("퀘스트 클리어");
        QuestDestroy();
        QuestExit();
    }

    private void QuestExit()
    {
        //UpdateQuest();

        for (int i = 4; i >= 0; i--)
        {
            if (questUIdatas[i].gameObject.activeSelf)
            {
                questUIdatas[i].QuestEnd();

                return;
            }
        }
        

        //for (int i = 4; i >= 0; i++)
        //{
        //    //questTexts[i].gameObject.SetActive(false);
        //    //questTexts[i].transform.parent.gameObject.SetActive(false);
        //    questUIdatas[i].QuestEnd();
        //}
    }

    public void QuestInput(CurrentQuest quest)
    {
        //UpdateQuest();
        titleText.text = quest.QuestTitleText;
        curQuest = quest.curQuest;
        titleLogo.DORestartById("Start");
        QuestSetText();
        questUIdatas[0].GetComponent<DOTweenAnimation>().DORestartById("Start");
    }

    private void QuestSetText()
    {
        int count = 0;
        //UpdateQuest();
        while (curQuest != null && count < curQuest.QuestDatas.Length)
        {
            int temp = count + 1;
            if (curQuest.QuestDatas[count].type == QuestType.Chat || curQuest.QuestDatas[count].type == QuestType.Interaction || curQuest.QuestDatas[count].type == QuestType.Position)
                questUIdatas[count].text.text = curQuest.QuestDatas[count].questString;
            else if (curQuest.QuestDatas[count].type == QuestType.Enemy)
                questUIdatas[count].text.text = curQuest.QuestDatas[count].questString + "(" + curQuest.QuestDatas[count].enemyCurCount.ToString() + " / " + curQuest.QuestDatas[count].enemyCompleteCount.ToString() + ")";
            else if (curQuest.QuestDatas[count].type == QuestType.Item)
                questUIdatas[count].text.text = curQuest.QuestDatas[count].questString + "(" + curQuest.QuestDatas[count].itemCurCount.ToString() + " / " + curQuest.QuestDatas[count].itemCompleteCount.ToString() + ")";

            //questTexts[count].gameObject.SetActive(true);
            //questTexts[count].transform.parent.gameObject.SetActive(true);
            questUIdatas[count].gameObject.SetActive(true);
            
            count++;
        }
    }

    private void QuestText()
    {
        int count = 0;
        //UpdateQuest();
        while (curQuest != null && count < curQuest.QuestDatas.Length)
        {
            int temp = count + 1;
            if ((curQuest.QuestDatas[count].type == QuestType.Chat || curQuest.QuestDatas[count].type == QuestType.Interaction || curQuest.QuestDatas[count].type == QuestType.Position) && !curQuest.QuestDatas[count].clear)
                questUIdatas[count].text.text = curQuest.QuestDatas[count].questString;
            else if (curQuest.QuestDatas[count].type == QuestType.Enemy)
                questUIdatas[count].text.text = curQuest.QuestDatas[count].questString + "(" + curQuest.QuestDatas[count].enemyCurCount.ToString() + " / " + curQuest.QuestDatas[count].enemyCompleteCount.ToString() + ")";
            else if (curQuest.QuestDatas[count].type == QuestType.Item)
                questUIdatas[count].text.text = curQuest.QuestDatas[count].questString + "(" + curQuest.QuestDatas[count].itemCurCount.ToString() + " / " + curQuest.QuestDatas[count].itemCompleteCount.ToString() + ")";

            //questTexts[count].gameObject.SetActive(true);
            //questTexts[count].transform.parent.gameObject.SetActive(true);
            //questUIdatas[count].gameObject.SetActive(true);
            count++;
        }

    }

    public void ChatQuestCheck(string chatKey)
    {
        //UpdateQuest();

        if (curQuest == null) return;

        int count = 0;

        while (curQuest != null && count < curQuest.QuestDatas.Length)
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
        //UpdateQuest();

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
        //UpdateQuest();
        if (curQuest == null) return;

        int count = 0;

        while (curQuest != null && count < curQuest.QuestDatas.Length)
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

    private IEnumerator EnemyCheckCor(string name)
    {
        yield return null;
        //UpdateQuest();
        //if (curQuest == null) StopCoroutine(EnemyCheckCor(name));

        int count = 0;

        while (curQuest != null && count < curQuest.QuestDatas.Length)
        {
            if (curQuest.QuestDatas[count].type == QuestType.Enemy)
            {
                if (!curQuest.QuestDatas[count].clear && name == curQuest.QuestDatas[count].enemyName)
                {
                    curQuest.QuestDatas[count].enemyCurCount++;
                    //Debug.LogError("enemyQuestCheck Count : " + curQuest.QuestDatas[count].enemyCurCount);

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

    public void EnemyQuestCheck(string name)
    {
        //Debug.LogError("enemyQuestCheck name : " + name);
        StartCoroutine(EnemyCheckCor(name));

        //UpdateQuest();
        //if (curQuest == null) return;

        //int count = 0;

        //while (curQuest != null && count < curQuest.QuestDatas.Length)
        //{
        //    if (curQuest.QuestDatas[count].type == QuestType.Enemy)
        //    {
        //        if (!curQuest.QuestDatas[count].clear && name == curQuest.QuestDatas[count].enemyName)
        //        {
        //            curQuest.QuestDatas[count].enemyCurCount++;

        //            if (curQuest.QuestDatas[count].enemyCurCount >= curQuest.QuestDatas[count].enemyCompleteCount)
        //            {
        //                QuestClear(curQuest.QuestDatas[count], count);
        //            }
        //        }
        //    }
        //    QuestText();
        //    count++;
        //}


    }

    public void PositionQuestCheck(string name)
    {
        //UpdateQuest();
        if (curQuest == null) return;

        int count = 0;

        while (curQuest != null && count < curQuest.QuestDatas.Length)
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


