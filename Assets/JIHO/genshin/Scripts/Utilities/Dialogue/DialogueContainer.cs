using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class DialogueContainer : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, DialogueSequence[]> dialogues;
    [SerializeField] private Dictionary<string, LargeDialogueData[]> cutscene_dialogues;

    private Player player;

    public void StartDialogue(string key)
    {
        UI_Dialogue dialogueUI = GameObject.FindObjectOfType<UI_Dialogue>();
        if (dialogueUI == null) { Debug.LogError("UI_Dialogue�� �����ϴ� ���ӿ�����Ʈ�� ã�� �� �����ϴ�. �ش� ��ũ��Ʈ�� ���� ������Ʈ�� �Բ� ����� �ּ���."); return; }

        if (player == null) player = FindObjectOfType<Player>();

        dialogueUI.PlayDialogue(dialogues[key]);

        if (QuestManager.instance != null)
            QuestManager.instance.QuestChatCheck(key);
    }

    public void StartLargeDialogue(string key)
    {
        UI_LargeDialogue dialogueUI = GameObject.FindObjectOfType<UI_LargeDialogue>();
        if (dialogueUI == null) { Debug.LogError("UI_LargeDialogue�� �����ϴ� ���ӿ�����Ʈ�� ã�� �� �����ϴ�. �ش� ��ũ��Ʈ�� ���� ������Ʈ�� �Բ� ����� �ּ���."); return; }

        dialogueUI.PlayDialogue(cutscene_dialogues[key]);
        if (QuestManager.instance != null)
            QuestManager.instance.QuestChatCheck(key);
    }
}


