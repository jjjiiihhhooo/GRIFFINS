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
    [Title("����Ʈ Ÿ��")]
    [InfoBox("Ÿ�Կ� �´� ����Ʈ�� �̿�")]
    public QuestType type;


    [TabGroup("��ȭ ����Ʈ")]
    [Header("��ȭ Ű��")]
    public string chatKey;

    [TabGroup("������ ����Ʈ")]
    [Header("������ �̸�")]
    public string itemName;

    [TabGroup("������ ����Ʈ")]
    [Header("������ Ŭ���� ����")]
    public int itemCompleteCount;

    [TabGroup("������ ����Ʈ")]
    [Header("������ ���� ����")]
    [ReadOnly] public int itemCurCount;


    [TabGroup("��ġ�̵� ����Ʈ")]
    [Header("��ġ �̸�")]
    public string posName;


    [TabGroup("���� ����Ʈ")]
    [Header("���� �̸�")]
    public string enemyName;

    [TabGroup("���� ����Ʈ")]
    [Header("���� Ŭ���� ������")]
    public int enemyCompleteCount;

    [TabGroup("���� ����Ʈ")]
    [Header("���� ���� ������")]
    [ReadOnly] public int enemyCurCount;

    [TabGroup("��ȣ�ۿ� ����Ʈ")]
    [Header("��ȣ�ۿ� �̸�")]
    public string interactionName;

    [Header("����Ʈ ����")]
    public string questString;
    [Header("Ŭ�����")]
    [ReadOnly] public bool clear;

    public UnityEvent clear_event = null;
}
