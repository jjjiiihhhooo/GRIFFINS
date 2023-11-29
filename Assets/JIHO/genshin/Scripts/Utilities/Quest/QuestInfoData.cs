using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace genshin
{
    public enum QuestType
    {
        Chat, Position, item, Monster
    }

    [System.Serializable]
    public class QuestInfo
    {

        [System.Serializable]
        public class QuestInfoData
        {
            [Header("�ش���� ���� Ÿ���� ������ �Է� �ʿ� ����")]
            [Header("����Ʈ �ʼ� ���� (Ÿ��, �̸�, ����)")]
            public QuestType questType;
            public string title;
            public string description;
            public bool isClear;

            [Header("��ġ����Ʈ ����[Position] (�����ؾ��ϴ� ��ġ�� �̸�)")]
            public string position;

            [Header("����������Ʈ ����[Item] (��ǥ ������, ��ǥ����, ���簹���� x)")]
            //public Ingredient_Item item;
            public int itemCompleteCount;
            public int itemCurrentCount;

            [Header("���ͻ�� ����Ʈ ����[Monster]")]
            public string monsterName;
            public int monsterCompleteCount;
            public int monsterCurrentCount;

            [Header("��ȭ����Ʈ ����[Chat] (��ȭ Ű��)")]
            public string chatKey;

            public UnityEvent action;
        }
        [Header("------------------------����Ʈ ����------------------------")]
        public QuestInfoData[] questInfoDatas = new QuestInfoData[1];
        public UnityEvent action;
    }

}
