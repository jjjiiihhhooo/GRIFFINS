using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class QuestManager : SerializedMonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] private Dictionary<string, QuestInfo> questMap = new Dictionary<string, QuestInfo>();

    [SerializeField] private QuestUI questUI;
    [SerializeField] private DOTweenAnimation titleDotweenAni;
    [SerializeField] private DOTweenAnimation clearDotweenAni;
    [SerializeField] private UnityEngine.UI.Outline questOutline;

    private bool isQuestActive = false;

    public QuestInfo currentQuest;
    public QuestInfo tempQuest;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            currentQuest = null;
            tempQuest = null;
            DontDestroyOnLoad(gameObject);
            titleDotweenAni = GetComponentInChildren<DOTweenAnimation>();
            QuestInput("Tutorial_Quest_1");
        }
        else
        {
            Destroy(gameObject);
        }
        //QuestInput("????????");

    }

    private void Update()
    {
        //if (UIManager.Instance.isControl == true)
        //{
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    QuestActive();
        //    if (questUI.checking_Quest == false) questUI.checking_Quest = true;
        //}

        //if (currentQuest == null && tempQuest != null)
        //{
        //    QuestInput(tempQuest.questInfoDatas[0].title);
        //    tempQuest = null;
        //}
        //}

    }

    private void QuestActive()
    {
        //if (isQuestActive)
        //{
        //    titleDotweenAni.DOPause();
        //    titleDotweenAni.DORestartAllById("false");
        //    isQuestActive = !isQuestActive;
        //}
        //else if (!isQuestActive)
        //{
        //    titleDotweenAni.DOPause();
        //    titleDotweenAni.DORestartAllById("true");
        //    isQuestActive = !isQuestActive;
        //}
    }

    private void QuestClearCheck()
    {
        questUI.UpdateUI();

        for (int i = 0; i < currentQuest.questInfoDatas.Length; i++)
            if (!currentQuest.questInfoDatas[i].isClear) return;

        if (currentQuest.action != null)
            currentQuest.action.Invoke();

        currentQuest = null;
        questUI.SetActiveQuest(5, false);



        Debug.Log("Äù½ºÆ® Ã¼Å©");


        Debug.Log(QuestAllClear());

        if (QuestAllClear()) StartCoroutine(QuestClearAni());
        //StopAllCoroutines();
        //else StartCoroutine(QuestClearAni());


    }

    public void QusetPanelHide()
    {
        if (isQuestActive)
        {
            titleDotweenAni.DOPause();
            titleDotweenAni.DORestartAllById("false");
            isQuestActive = !isQuestActive;
        }
        else return;
    }

    private bool QuestAllClear()
    {
        foreach (KeyValuePair<string, QuestInfo> pair in questMap)
        {
            for (int i = 0; i < pair.Value.questInfoDatas.Length; i++)
            {
                if (!pair.Value.questInfoDatas[i].isClear)
                {
                    return false;
                }
            }
        }
        return true;
    }
    private IEnumerator QuestClearAni()
    {
        clearDotweenAni.DORestartById("in");
        yield return new WaitForSeconds(1f);
        clearDotweenAni.DORestartById("out");
    }

    public void QuestInput(string id)
    {
        Debug.Log("dd");
        if (!questMap.ContainsKey(id))
        {
            return;
        }

        if (currentQuest != null)
        {
            tempQuest = questMap[id];
            tempQuest.questInfoDatas[0].title = id;
            return;
        }

        currentQuest = questMap[id];
        questUI.ReRoadUI(currentQuest);
        isQuestActive = false;
        QuestActive();
        //StartCoroutine(QuestInptAnim());
        //StartCoroutine(questUI.ViewOutLine());
    }

    private IEnumerator QuestInptAnim()
    {
        for (int i = 0; i < 3; i++)
        {
            questOutline.enabled = true;
            yield return new WaitForSeconds(0.3f);
            questOutline.enabled = false;
            yield return new WaitForSeconds(0.3f);
        }
        if (isQuestActive)
        {
            QuestActive();
            questUI.checking_Quest = false;
        }
    }

    public void QuestItemCheck()
    {
        //if (currentQuest == null) return;

        //int i = 0;
        //while (currentQuest != null && i < currentQuest.questInfoDatas.Length)
        //{

        //    if (currentQuest.questInfoDatas[i].questType == QuestType.item)
        //        if (currentQuest.questInfoDatas[i].item.count >= currentQuest.questInfoDatas[i].itemCompleteCount)
        //        {
        //            if (currentQuest.questInfoDatas[i].isClear)
        //            {
        //                i++;
        //                continue;
        //            }
        //            currentQuest.questInfoDatas[i].isClear = true;
        //            Debug.Log("Äù½ºÆ® ¿Ï·á");
        //            if (currentQuest.questInfoDatas[i].action != null)
        //                currentQuest.questInfoDatas[i].action.Invoke();
        //        }
        //    i++;
        //}
        //QuestClearCheck();
    }

    public void QuestPositionCheck(string name)
    {
        if (currentQuest == null) return;

        int i = 0;
        while (currentQuest != null && i < currentQuest.questInfoDatas.Length)
        {
            if (currentQuest.questInfoDatas[i].questType == QuestType.Position)
                if (currentQuest.questInfoDatas[i].position == name)
                {
                    if (currentQuest.questInfoDatas[i].isClear)
                    {
                        i++;
                        continue;
                    }
                    currentQuest.questInfoDatas[i].isClear = true;
                    Debug.Log("Äù½ºÆ® ¿Ï·á");
                    if (currentQuest.questInfoDatas[i].action != null)
                        currentQuest.questInfoDatas[i].action.Invoke();
                }
            i++;
        }

        QuestClearCheck();
    }

    public void QuestChatCheck(string key)
    {
        if (currentQuest == null) return;

        int i = 0;
        while (currentQuest != null && i < currentQuest.questInfoDatas.Length)
        {
            if (currentQuest.questInfoDatas[i].chatKey == key)
            {
                if (currentQuest.questInfoDatas[i].isClear)
                {
                    i++;
                    continue;
                }
                currentQuest.questInfoDatas[i].isClear = true;
                Debug.Log("Äù½ºÆ® Ã¼Å©");
                if (currentQuest.questInfoDatas[i].action != null)
                    currentQuest.questInfoDatas[i].action.Invoke();
                QuestClearCheck();
            }
            i++;
        }
    }

    public void QuestMonsterCheck(string name)
    {
        if (currentQuest == null) return;

        int i = 0;
        while (currentQuest != null && i < currentQuest.questInfoDatas.Length)
        {
            if (currentQuest.questInfoDatas[i].questType == QuestType.Monster)
                if (currentQuest.questInfoDatas[i].monsterName == name)
                {
                    if (currentQuest.questInfoDatas[i].isClear)
                    {
                        i++;
                        continue;
                    }

                    currentQuest.questInfoDatas[i].monsterCurrentCount++;
                    if (currentQuest.questInfoDatas[i].monsterCurrentCount >= currentQuest.questInfoDatas[i].monsterCompleteCount)
                    {
                        currentQuest.questInfoDatas[i].isClear = true;

                        Debug.Log("Äù½ºÆ® ¿Ï·á");
                        if (currentQuest.questInfoDatas[i].action != null)
                            currentQuest.questInfoDatas[i].action.Invoke();
                    }
                }
            i++;
        }

        QuestClearCheck();
    }

    public void ResetQuestCheck()
    {
        Debug.Log("Äù½ºÆ® ÃÊ±âÈ­");
        foreach (KeyValuePair<string, QuestInfo> pair in questMap)
        {
            for (int i = 0; i < pair.Value.questInfoDatas.Length; i++)
            {
                pair.Value.questInfoDatas[i].isClear = false;
            }
        }

    }
    public void CurrentAndTempQuestClear()
    {
        currentQuest = null;
        tempQuest = null;
    }
}


