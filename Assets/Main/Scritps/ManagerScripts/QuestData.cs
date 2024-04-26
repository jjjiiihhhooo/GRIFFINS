using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum QuestType
{
    Chat, Item, Position, Enemy, Interaction
}


[System.Serializable]
public class QuestBox
{
    public QuestData[] QuestDatas;
    public UnityEvent clear_event = null;
}


[System.Serializable]
public class QuestData
{
    [Title("퀘스트 타입")]
    [InfoBox("타입에 맞는 퀘스트만 이용")]
    public QuestType type;


    [TabGroup("대화 퀘스트")]
    [Header("대화 키값")]
    public string chatKey;

    [TabGroup("아이템 퀘스트")]
    [Header("아이템 이름")]
    public string itemName;

    [TabGroup("아이템 퀘스트")]
    [Header("아이템 클리어 갯수")]
    public int itemCompleteCount;

    [TabGroup("아이템 퀘스트")]
    [Header("아이템 현재 갯수")]
    [ReadOnly] public int itemCurCount;


    [TabGroup("위치이동 퀘스트")]
    [Header("위치 이름")]
    public string posName;


    [TabGroup("몬스터 퀘스트")]
    [Header("몬스터 이름")]
    public string enemyName;

    [TabGroup("몬스터 퀘스트")]
    [Header("몬스터 클리어 마리수")]
    public int enemyCompleteCount;

    [TabGroup("몬스터 퀘스트")]
    [Header("몬스터 현재 마리수")]
    [ReadOnly] public int enemyCurCount;

    [TabGroup("상호작용 퀘스트")]
    [Header("상호작용 이름")]
    public string interactionName;

    [Header("퀘스트 설명")]
    public string questString;
    [Header("클리어여부")]
    [ReadOnly] public bool clear;

    public UnityEvent clear_event = null;
}
